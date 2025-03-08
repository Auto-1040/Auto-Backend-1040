using Auto1040.Api.PostModels;
using Auto1040.Core.DTOs;
using Auto1040.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Auto1040.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDetailsController : ControllerBase
    {
        private readonly IUserDetailsService _userDetailsService;
        private readonly IMapper _mapper;

        public UserDetailsController(IUserDetailsService userDetailsService, IMapper mapper)
        {
            _userDetailsService = userDetailsService;
            _mapper = mapper;
        }


        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public ActionResult<IEnumerable<UserDetailsDto>> GetAll()
        {
            var result = _userDetailsService.GetAllUserDetails();
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<UserDetailsDto> Get(int id)
        {
            var userId = GetUserId();
            if (!_userDetailsService.IsUserDetailsOwner(id, userId))
                return Forbid("You are not authorized to access these details.");

            var result = _userDetailsService.GetUserDetailsById(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize]
        public ActionResult<bool> Add([FromBody] UserDetailsPostModel userDetails)
        {
            if (userDetails == null)
                return BadRequest("User details data is required.");

            var userId = GetUserId();
            if (userDetails.UserId != userId)
                return Forbid("You are not authorized to add these details.");

            var userDetailsDto = _mapper.Map<UserDetailsDto>(userDetails);
            var result = _userDetailsService.AddUserDetails(userDetailsDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<bool> Update(int id, [FromBody] UserDetailsPostModel userDetails)
        {
            if (userDetails == null)
                return BadRequest("User details data is required.");

            var userId = GetUserId();
            if (!_userDetailsService.IsUserDetailsOwner(id, userId))
                return Forbid("You are not authorized to update these details.");

            var userDetailsDto = _mapper.Map<UserDetailsDto>(userDetails);
            var result = _userDetailsService.UpdateUserDetails(id, userDetailsDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<bool> Delete(int id)
        {
            var userId = GetUserId();
            if (!_userDetailsService.IsUserDetailsOwner(id, userId))
                return Forbid("You are not authorized to delete these details.");

            var result = _userDetailsService.DeleteUserDetails(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return NoContent();
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub));
        }
    }
}
