using Auto1040.Core.DTOs;
using System.Collections.Generic;

namespace Auto1040.Core.Services
{
    public interface IUserService
    {
        Result<IEnumerable<UserDto>> GetAllUsers();
        Result<UserDto> GetUserById(int id);
        Result<bool> AddUser(UserDto userDto);
        Result<bool> UpdateUser(int id, UserDto userDto);
        Result<bool> DeleteUser(int id);
        bool IsUserAdmin(int userId);

    }
}
