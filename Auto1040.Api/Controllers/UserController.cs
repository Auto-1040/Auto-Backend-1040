using Auto1040.Api.PostModels;
using Auto1040.Core.DTOs;
using Auto1040.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Auto1040.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy ="AdminOnly")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> Get()
        {
            var result = _userService.GetAllUsers();
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> Get(int id)
        {
            var result = _userService.GetUserById(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPost]
        public ActionResult<bool> Add([FromBody] UserPostModel user)
        {
            if (user == null)
                return BadRequest("User data is required.");

            var userDto = _mapper.Map<UserDto>(user);
            var result = _userService.AddUser(userDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public ActionResult<bool> Update(int id, [FromBody] UserPostModel user)
        {
            if (user == null)
                return BadRequest("User data is required.");

            var userDto = _mapper.Map<UserDto>(user);
            var result = _userService.UpdateUser(id, userDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(int id)
        {
            var result = _userService.DeleteUser(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return NoContent();
        }
    }
}
