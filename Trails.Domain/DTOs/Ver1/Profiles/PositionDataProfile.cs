using System.Globalization;
using AutoMapper;
using Trails.Domain.DTOs.Ver1.PositionDataDTOs.Requests;
using Trails.Domain.DTOs.Ver1.PositionDataDTOs.Responses;
using Trails.Domain.Models;


namespace Trails.Domain.DTOs.Ver1.Profiles
{
    public class PositionDataProfile : Profile
    {
        public PositionDataProfile()
        {
            CreateMap<PositionData, PositionDataGet>()
                .ForMember(x => x.Timestamp,
                    y => y.MapFrom(s => s.Timestamp.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)));
            CreateMap<PositionDataPost, PositionData>();
        }
    }
}
