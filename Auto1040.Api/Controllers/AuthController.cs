using Auto1040.Api.PostModels;
using Auto1040.Core.DTOs;
using Auto1040.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Auto1040.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService,IMapper mapper) : ControllerBase
    {
        private readonly IAuthService _authService=authService;
        private readonly IMapper _mapper = mapper;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var result = _authService.Login(model.UserNameOrEmail, model.Password);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return Unauthorized(result.ErrorMessage);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserPostModel user)
        {
            if (user == null)
                return BadRequest("User data is required.");

            var userDto = _mapper.Map<UserDto>(user);
            var result = _authService.Register(userDto);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
    }

    public class LoginModel
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
