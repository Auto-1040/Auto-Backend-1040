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
    public class AuthService(IConfiguration configuration, IRepositoryManager repositoryManager,IMapper mapper) : IAuthService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;

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
            if (user.Roles != null)
            {
                foreach (var role in user.Roles.Select(r => r.RoleName))
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
                var userDto = _mapper.Map<UserDto>(user);
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
            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                HashedPassword = userDto.HashedPassword,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            if (_repositoryManager.Users.GetList().Any(u => u.UserName == user.UserName||u.UserName==user.Email || u.Email == user.Email||u.Email==user.UserName))
            {
                return Result<LoginResponseDto>.Failure("Username or email already exists.");
            }

            user.Roles = new List<Role>();
            foreach (var role in userDto.Roles)
            {
                var userRole = _repositoryManager.Roles.GetByName(role);
                if (userRole != null)
                {
                    user.Roles.Add(userRole);
                }
            }
           
            var result = _repositoryManager.Users.Add(user);
            if (result == null)
            {
                return Result<LoginResponseDto>.Failure("Failed to register user.");
            }
            _repositoryManager.Save();

            var token = GenerateJwtToken(result);

            var responseUserDto = _mapper.Map<UserDto>(result);
            var response = new LoginResponseDto
            {
                User = responseUserDto,
                Token = token
            };
            return Result<LoginResponseDto>.Success(response);
        }
    }
}
