using AutoMapper;
using Trails.Domain.DTOs.Ver1.DeviceDTOs.Requests;
using Trails.Domain.DTOs.Ver1.DeviceDTOs.Responses;
using Trails.Domain.Models;

namespace Trails.Domain.DTOs.Ver1.Profiles
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<Device, DeviceGet>();
            CreateMap<DevicePost, Device>();
            CreateMap<DevicePut, Device>();
        }
    }
}
