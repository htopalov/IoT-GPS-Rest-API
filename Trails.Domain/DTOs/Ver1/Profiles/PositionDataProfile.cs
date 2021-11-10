using AutoMapper;
using Trails.Domain.DTOs.Ver1.PositionDataDTOs.Requests;
using Trails.Domain.DTOs.Ver1.PositionDataDTOs.Responses;
using Trails.Domain.Models;


namespace Trails.Domain.DTOs.Ver1.Profiles
{
    class PositionDataProfile : Profile
    {
        public PositionDataProfile()
        {
            CreateMap<PositionData, PositionDataGet>();
            CreateMap<PositionDataPost, PositionData>();
        }
    }
}
