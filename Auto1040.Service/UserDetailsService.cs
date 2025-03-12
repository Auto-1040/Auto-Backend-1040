using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;
using Auto1040.Core.Services;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Auto1040.Service
{
    public class UserDetailsService(IRepositoryManager repositoryManager, IMapper mapper) : IUserDetailsService
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;

        public Result<IEnumerable<UserDetailsDto>> GetAllUserDetails()
        {
            var result = _mapper.Map<List<UserDetailsDto>>(_repositoryManager.UserDetails.GetList());
            return Result<IEnumerable<UserDetailsDto>>.Success(result);
        }

        public Result<UserDetailsDto> GetUserDetailsByUserId(int userId)
        {
            var userDetails = _repositoryManager.UserDetails.GetByUserId(userId);
            if (userDetails == null)
                return Result<UserDetailsDto>.NotFound("User details not found");
            return Result<UserDetailsDto>.Success(_mapper.Map<UserDetailsDto>(userDetails));
        }

        public Result<UserDetailsDto> AddUserDetails(UserDetailsDto userDetailsDto)
        {
            var userDetails = _mapper.Map<UserDetails>(userDetailsDto);
            if (userDetails == null)
                return Result<UserDetailsDto>.BadRequest("Cannot add user details of null reference");

            var user = _repositoryManager.Users.GetById(userDetails.UserId);
            if(user == null)
                return Result<UserDetailsDto>.BadRequest("User not found");

            var details= _repositoryManager.UserDetails.GetByUserId(userDetails.UserId);
            if (details != null)
                return Result<UserDetailsDto>.BadRequest("User details already exist");

            if (!IsValidSsn(userDetails.Ssn))
                return Result<UserDetailsDto>.BadRequest("Invalid SSN format.");

            if (userDetails.SpouseSsn != null && !IsValidSsn(userDetails.SpouseSsn))
                return Result<UserDetailsDto>.BadRequest("Invalid spouse SSN format.");

            var result = _repositoryManager.UserDetails.Add(userDetails);
            if (result == null)
                return Result<UserDetailsDto>.Failure("Failed to add user details");

            _repositoryManager.Save();
            var resultDto=_mapper.Map<UserDetailsDto>(result);
            return Result<UserDetailsDto>.Success(resultDto);
        }

        public Result<UserDetailsDto> UpdateUserDetailsByUserId(int id, UserDetailsDto userDetailsDto)
        {
            var userDetails = _mapper.Map<UserDetails>(userDetailsDto);
            if (userDetails == null)
                return Result<UserDetailsDto>.BadRequest("Cannot update user details to null reference");

            if (userDetails.Ssn!=null&&!IsValidSsn(userDetails.Ssn))
                return Result<UserDetailsDto>.BadRequest("Invalid SSN format.");

            if (userDetails.SpouseSsn != null && !IsValidSsn(userDetails.SpouseSsn))
                return Result<UserDetailsDto>.BadRequest("Invalid spouse SSN format.");

            var existingUserDetails = _repositoryManager.UserDetails.GetByUserId(id);
            if (existingUserDetails == null)
                return Result<UserDetailsDto>.NotFound($"Id {id} is not found");

            var result = _repositoryManager.UserDetails.UpdateByUserId(id, userDetails);
            if (result == null)
                return Result<UserDetailsDto>.Failure("Failed to update user details");

            _repositoryManager.Save();

            return Result<UserDetailsDto>.Success(_mapper.Map<UserDetailsDto>(result));
        }

        public Result<bool> DeleteUserDetailsByUserId(int userId)
        {
            if (_repositoryManager.UserDetails.GetByUserId(userId) == null)
                return Result<bool>.NotFound($"User Id {userId} is not found");

            var result = _repositoryManager.UserDetails.DeleteByUserId(userId);
            if (!result)
                return Result<bool>.Failure("Failed to delete user details");

            _repositoryManager.Save();
            return Result<bool>.Success(result);
        }

        public bool IsUserDetailsOwner(int userDetailsId, int userId)
        {
            return userDetailsId == userId;
        }

        private bool IsValidSsn(string ssn)
        {
            if (string.IsNullOrWhiteSpace(ssn))
                return true;

            var ssnRegex = new Regex(@"^\d{3}-\d{2}-\d{4}$", RegexOptions.Compiled);
            return ssnRegex.IsMatch(ssn);
        }
    }
}

