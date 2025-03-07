using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto1040.Core
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<UserDetailsDto, UserDetails>().ReverseMap();
        }
    }
}
