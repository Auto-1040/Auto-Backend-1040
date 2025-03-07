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
    public class UserService(IRepositoryManager repositoryManager, IMapper mapper) : IUserService
    {
        private readonly IRepositoryManager _repositoryManager=repositoryManager;
        private readonly IMapper _mapper=mapper;

        public Result<IEnumerable<UserDto>> GetAllUsers()
        {
            var result = _mapper.Map<List<UserDto>>(_repositoryManager.Users.GetList());
            return Result<IEnumerable<UserDto>>.Success(result);
        }

        public Result<UserDto> GetUserById(int id)
        {
            var user = _repositoryManager.Users.GetById(id);
            if (user == null)
                return Result<UserDto>.NotFound("User not found");
            return Result<UserDto>.Success(_mapper.Map<UserDto>(user));
        }

        public Result<bool> AddUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            if (user == null)
                return Result<bool>.BadRequest("Cannot add user of null reference");

            if (!IsValidEmail(user.Email))
                return Result<bool>.BadRequest("Invalid email format.");
            if(user.UserName == null)
                return Result<bool>.BadRequest("Username is required.");

            if (_repositoryManager.Users.GetList().Any(u => u.Email == user.Email))
                return Result<bool>.BadRequest("A user with this email already exists.");

            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            var result = _repositoryManager.Users.Add(user);
            if (result == null)
                return Result<bool>.Failure("Failed to add user");

            _repositoryManager.Save();
            return Result<bool>.Success(true);
        }

        public Result<bool> UpdateUser(int id, UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            if (user == null)
                return Result<bool>.BadRequest("Cannot update user to null reference");

            if (user.Email!=null&&!IsValidEmail(user.Email))
                return Result<bool>.BadRequest("Invalid email format.");

            var existingUser = _repositoryManager.Users.GetById(id);
            if (existingUser == null)
                return Result<bool>.NotFound($"Id {id} is not found");

            if (_repositoryManager.Users.GetList().Any(u => u.Email == user.Email && u.Id != id))
                return Result<bool>.BadRequest("A user with this email already exists.");

            user.UpdatedAt = DateTime.UtcNow;

            var result = _repositoryManager.Users.Update(id, user);
            if (result == null)
                return Result<bool>.Failure("Failed to update user");

            _repositoryManager.Save();
            return Result<bool>.Success(true);
        }

        public Result<bool> DeleteUser(int id)
        {
            if (_repositoryManager.Users.GetById(id) == null)
                return Result<bool>.NotFound($"Id {id} is not found");

            var result = _repositoryManager.Users.Delete(id);
            if (!result)
                return Result<bool>.Failure("Failed to delete user");

            _repositoryManager.Save();
            return Result<bool>.Success(result);
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
                return emailRegex.IsMatch(email);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}

