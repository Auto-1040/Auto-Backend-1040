using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;
using Auto1040.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using AutoMapper;

namespace Auto1040.Service
{
    public class AuthService(IConfiguration configuration, IRepositoryManager repositoryManager,IUserService userService, IMapper mapper) : IAuthService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;

        public string GenerateJwtToken(UserDto user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add roles as claims
            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateUser(string usernameOrEmail, string password, out User user)
        {
            user = _repositoryManager.Users.GetUserWithRoles(usernameOrEmail);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.HashedPassword))
                return true;

            return false;
        }


        public Result<LoginResponseDto> Login(string usernameOrEmail, string password)
        {
            if (ValidateUser(usernameOrEmail, password, out var user))
            {
                var userDto = _mapper.Map<UserDto>(user);
                var token = GenerateJwtToken(userDto);

                var response = new LoginResponseDto
                {
                    User = userDto,
                    Token = token
                };
                return Result<LoginResponseDto>.Success(response);
            }

            return Result<LoginResponseDto>.Failure("Invalid username or password.");
        }

        public Result<LoginResponseDto> Register(UserDto userDto)
        {
            userDto.Roles = new List<string> { "User" };

            var addUserResult = _userService.AddUser(userDto);
            if (!addUserResult.IsSuccess)
                return Result<LoginResponseDto>.Failure(addUserResult.ErrorMessage);


            if (addUserResult.Data == null)
                return Result<LoginResponseDto>.Failure("Failed to load user after registration.");

            var token = GenerateJwtToken(addUserResult.Data);

            var response = new LoginResponseDto
            {
                User = addUserResult.Data,
                Token = token
            };

            return Result<LoginResponseDto>.Success(response);
        }
    }
}
