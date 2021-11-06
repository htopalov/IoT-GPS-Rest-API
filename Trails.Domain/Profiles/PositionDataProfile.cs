using AutoMapper;
using Trails.Domain.DTOs.PositionData;
using Trails.Domain.Models;

namespace Trails.Domain.Profiles
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
