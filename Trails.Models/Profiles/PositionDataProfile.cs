using AutoMapper;
using Trails.Models.DTOs;

namespace Trails.Models.Profiles
{
    public class PositionDataProfile : Profile
    {
        public PositionDataProfile()
        {
            CreateMap<PositionData, PositionDataDtoGet>();
            CreateMap<PositionDataDtoPost, PositionData>();
        }
    }
}
