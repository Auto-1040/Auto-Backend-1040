using AutoMapper;
using Leyadech.Api.PostModels;
using Leyadech.Core.DTOs;
using Leyadech.Core.Entities;

namespace Leyadech.Api
{
    public class MappingPostProfile:Profile
    {
        public MappingPostProfile()
        {
            CreateMap<MotherPostModel, MotherDto>();
        }
    }
}
