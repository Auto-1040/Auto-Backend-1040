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

        [HttpGet("{userId}")]
        [Authorize]
        public ActionResult<UserDetailsDto> GetByUserId(int userId)
        {
            var authId = GetUserId();
            if (!_userDetailsService.IsUserDetailsOwner(userId,authId))
                return Forbid();

            var result = _userDetailsService.GetUserDetailsByUserId(userId);
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
                return Forbid();

            var userDetailsDto = _mapper.Map<UserDetailsDto>(userDetails);
            var result = _userDetailsService.AddUserDetails(userDetailsDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPut("{userId}")]
        [Authorize]
        public ActionResult<UserDetailsDto> UpdateByUserId(int userId, [FromBody] UserDetailsPostModel userDetails)
        {
            if (userDetails == null)
                return BadRequest("User details data is required.");

            var authId = GetUserId();
            if (!_userDetailsService.IsUserDetailsOwner(userId, authId))
                return Forbid();

            var userDetailsDto = _mapper.Map<UserDetailsDto>(userDetails);
            var result = _userDetailsService.UpdateUserDetailsByUserId(userId,userDetailsDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpDelete("{userId}")]
        [Authorize]
        public ActionResult<bool> DeleteByUserId(int userId)
        {
            var authId = GetUserId();
            if (!_userDetailsService.IsUserDetailsOwner(authId, userId))
                return Forbid();

            var result = _userDetailsService.DeleteUserDetailsByUserId(userId);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return NoContent();
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("User ID claim is missing.");
            }
            return int.Parse(userIdClaim);
        }



    }
}
