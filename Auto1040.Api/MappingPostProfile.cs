using Auto1040.Api.PostModels;
using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;
using AutoMapper;
using BCrypt.Net;

namespace Auto1040.Api
{
    public class MappingPostProfile : Profile
    {
        public MappingPostProfile()
        {
            CreateMap<UserPostModel, UserDto>()
                .ForMember(dest => dest.HashedPassword, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));
            CreateMap<UserDetailsPostModel, UserDetailsDto>();
            CreateMap<PaySlipPostModel, PaySlipDto>();

        }
    }
}
