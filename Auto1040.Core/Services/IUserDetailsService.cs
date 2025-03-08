using Auto1040.Core.DTOs;
using System.Collections.Generic;

namespace Auto1040.Core.Services
{
    public interface IUserDetailsService
    {
        Result<IEnumerable<UserDetailsDto>> GetAllUserDetails();
        Result<UserDetailsDto> GetUserDetailsById(int id);
        Result<bool> AddUserDetails(UserDetailsDto userDetailsDto);
        Result<bool> UpdateUserDetails(int id, UserDetailsDto userDetailsDto);
        Result<bool> DeleteUserDetails(int id);
        bool IsUserDetailsOwner(int userDetailsId, int userId);
    }
}

