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

        public Result<UserDetailsDto> GetUserDetailsById(int id)
        {
            var userDetails = _repositoryManager.UserDetails.GetById(id);
            if (userDetails == null)
                return Result<UserDetailsDto>.NotFound("User details not found");
            return Result<UserDetailsDto>.Success(_mapper.Map<UserDetailsDto>(userDetails));
        }

        public Result<bool> AddUserDetails(UserDetailsDto userDetailsDto)
        {
            var userDetails = _mapper.Map<UserDetails>(userDetailsDto);
            if (userDetails == null)
                return Result<bool>.BadRequest("Cannot add user details of null reference");

            if (!IsValidSsn(userDetails.Ssn))
                return Result<bool>.BadRequest("Invalid SSN format.");

            if (userDetails.SpouseSsn != null && !IsValidSsn(userDetails.SpouseSsn))
                return Result<bool>.BadRequest("Invalid spouse SSN format.");

            var result = _repositoryManager.UserDetails.Add(userDetails);
            if (result == null)
                return Result<bool>.Failure("Failed to add user details");

            _repositoryManager.Save();
            return Result<bool>.Success(true);
        }

        public Result<bool> UpdateUserDetails(int id, UserDetailsDto userDetailsDto)
        {
            var userDetails = _mapper.Map<UserDetails>(userDetailsDto);
            if (userDetails == null)
                return Result<bool>.BadRequest("Cannot update user details to null reference");

            if (userDetails.Ssn!=null&&!IsValidSsn(userDetails.Ssn))
                return Result<bool>.BadRequest("Invalid SSN format.");

            if (userDetails.SpouseSsn != null && !IsValidSsn(userDetails.SpouseSsn))
                return Result<bool>.BadRequest("Invalid spouse SSN format.");

            var existingUserDetails = _repositoryManager.UserDetails.GetById(id);
            if (existingUserDetails == null)
                return Result<bool>.NotFound($"Id {id} is not found");

            var result = _repositoryManager.UserDetails.Update(id, userDetails);
            if (result == null)
                return Result<bool>.Failure("Failed to update user details");

            _repositoryManager.Save();
            return Result<bool>.Success(true);
        }

        public Result<bool> DeleteUserDetails(int id)
        {
            if (_repositoryManager.UserDetails.GetById(id) == null)
                return Result<bool>.NotFound($"Id {id} is not found");

            var result = _repositoryManager.UserDetails.Delete(id);
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
                return false;

            var ssnRegex = new Regex(@"^\d{3}-\d{2}-\d{4}$", RegexOptions.Compiled);
            return ssnRegex.IsMatch(ssn);
        }
    }
}

