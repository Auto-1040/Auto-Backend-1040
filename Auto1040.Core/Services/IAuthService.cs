using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;

namespace Auto1040.Core.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        bool ValidateUser(string usernameOrEmail, string password, out User user);
        Result<LoginResponseDto> Login(string usernameOrEmail, string password);
        Result<LoginResponseDto> Register(UserDto userDto);
    }
}
