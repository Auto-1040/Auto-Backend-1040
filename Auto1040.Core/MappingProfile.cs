using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;
using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
                    .ForMember(dest => dest.Roles, opt => opt.Ignore()) 
                    .ReverseMap()
                    .ForMember(dest => dest.Roles, opt => opt.Ignore());
        CreateMap<UserDetails, UserDetailsDto>().ReverseMap();
        CreateMap<PaySlip, PaySlipDto>().ReverseMap();
        CreateMap<OutputForm, OutputFormDto>().ReverseMap();
    }
}
