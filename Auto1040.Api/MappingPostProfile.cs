using Auto1040.Api.PostModels;
using Auto1040.Core.DTOs;
using Auto1040.Core.Services;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;

namespace Auto1040.Api
{
    public class MappingPostProfile : Profile
    {
        public MappingPostProfile()
        {
            CreateMap<UserPostModel, UserDto>()
                .ForMember(dest => dest.HashedPassword, opt=>opt.MapFrom(src=>HashPassword(src.Password)));
            CreateMap<UserDetailsPostModel, UserDetailsDto>();
        }


        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }


    
}

