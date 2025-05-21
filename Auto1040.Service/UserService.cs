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
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;

        public Result<IEnumerable<UserDto>> GetAllUsers()
        {
            var result = _mapper.Map<List<UserDto>>(_repositoryManager.Users.GetAllUsersWithRoles());
            return Result<IEnumerable<UserDto>>.Success(result);
        }

        public Result<UserDto> GetUserById(int id)
        {
            var user = _repositoryManager.Users.GetById(id);
            if (user == null)
                return Result<UserDto>.NotFound("User not found");
            return Result<UserDto>.Success(_mapper.Map<UserDto>(user));
        }

        public Result<UserDto> AddUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            if (user == null)
                return Result<UserDto>.BadRequest("Cannot add user of null reference");

            if (!IsValidEmail(user.Email))
                return Result<UserDto>.BadRequest("Invalid email format.");
            if (!IsValidUserName(user.UserName))
                return Result<UserDto>.BadRequest("Invalid username format.");
            if (user.HashedPassword == null)
                return Result<UserDto>.BadRequest("Passsword is required.");

            if (_repositoryManager.Users.GetList().Any(u => u.Email == user.Email || u.Email == user.UserName || u.UserName == user.UserName || u.UserName == user.Email))
                return Result<UserDto>.BadRequest("A user with this username or email already exists.");

            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            //add roles
            user.Roles = new List<Role>();
            if (userDto.Roles != null)
            {
                foreach (var roleName in userDto.Roles)
                {
                    var role = _repositoryManager.Roles.GetByName(roleName);
                    if (role != null)
                        user.Roles.Add(role);
                }
            }

            var result = _repositoryManager.Users.Add(user);
            if (result == null)
                return Result<UserDto>.Failure("Failed to add user");

            _repositoryManager.Save();
            var resultDto = _mapper.Map<UserDto>(result);
            return Result<UserDto>.Success(resultDto);
        }

        public Result<bool> UpdateUser(int id, UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            if (user == null)
                return Result<bool>.BadRequest("Cannot update user to null reference");

            if (user.Email != null && !IsValidEmail(user.Email))
                return Result<bool>.BadRequest("Invalid email format.");
            if (user.UserName != null && !IsValidUserName(user.UserName))
                return Result<bool>.BadRequest("Invalid username format.");
            var existingUser = _repositoryManager.Users.GetById(id);
            if (existingUser == null)
                return Result<bool>.NotFound($"Id {id} is not found");

            if (_repositoryManager.Users.GetList().Any(u => u.Id != id && (u.Email == user.Email || u.Email == user.UserName || u.UserName == user.UserName || u.UserName == user.Email)))
                return Result<bool>.BadRequest("A user with this username or email already exists.");
            
            user.UpdatedAt = DateTime.UtcNow;

            if(userDto.Roles!=null)
            {
                user.Roles = new List<Role>();
                foreach (var roleName in userDto.Roles)
                {
                    var role = _repositoryManager.Roles.GetByName(roleName);
                    if (role != null)
                        user.Roles.Add(role);
                }
            }

            var result = _repositoryManager.Users.UpdateUserWithRoles(id, user);
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

        private bool IsValidUserName(string username)
        {
            if (string.IsNullOrEmpty(username))
                return false;
            if (username.Length < 2)
                return false;
            return true;

        }

        public bool IsUserAdmin(int userId)
        {
            var roles = _repositoryManager.Users.GetUserRoles(userId);
            if (roles == null)
                return false;

            return roles.Any(r => r.RoleName == "Admin");
        }
    }
}

