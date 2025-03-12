using Auto1040.Core.DTOs;
using System.Collections.Generic;

namespace Auto1040.Core.Services
{
    public interface IUserDetailsService
    {
        Result<IEnumerable<UserDetailsDto>> GetAllUserDetails();
        Result<UserDetailsDto> GetUserDetailsByUserId(int id);
        Result<UserDetailsDto> AddUserDetails(UserDetailsDto userDetailsDto);
        Result<UserDetailsDto> UpdateUserDetailsByUserId(int id, UserDetailsDto userDetailsDto);
        Result<bool> DeleteUserDetailsByUserId(int id);
        bool IsUserDetailsOwner(int userDetailsId, int userId);
    }
}

