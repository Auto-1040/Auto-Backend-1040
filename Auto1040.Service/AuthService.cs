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

namespace Auto1040.Service
{
    public class AuthService(IConfiguration configuration, IRepositoryManager repositoryManager) : IAuthService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IRepositoryManager _repositoryManager = repositoryManager;

        public string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add roles as claims
            foreach (var role in user.Roles.Select(r => r.RoleName))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
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

        public bool ValidateUser(string usernameOrEmail, string password, out string[] roles, out User user)
        {
            roles = null;
            user = _repositoryManager.Users.GetUserWithRoles(usernameOrEmail);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.HashedPassword))
            {
                roles = user.Roles.Select(r => r.RoleName).ToArray();
                return true;
            }

            return false;
        }


        public Result<LoginResponseDto> Login(string usernameOrEmail, string password)
        {
            if (ValidateUser(usernameOrEmail, password, out var roles, out var user))
            {
                var token = GenerateJwtToken(user);
                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    HashedPassword = user.HashedPassword
                };
                var response = new LoginResponseDto
                {
                    User = userDto,
                    Token = token
                };
                return Result<LoginResponseDto>.Success(response);
            }

            return Result<LoginResponseDto>.Failure("Invalid username or password.");
        }

        public Result<bool> Register(UserDto userDto)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                HashedPassword = userDto.HashedPassword,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            if (_repositoryManager.Users.GetList().Any(u => u.UserName == user.UserName||u.UserName==user.Email || u.Email == user.Email||u.Email==user.UserName))
            {
                return Result<bool>.Failure("Username or email already exists.");
            }

            var result = _repositoryManager.Users.Add(user);
            if (result == null)
            {
                return Result<bool>.Failure("Failed to register user.");
            }

            _repositoryManager.Save();
            return Result<bool>.Success(true);
        }
    }
}
