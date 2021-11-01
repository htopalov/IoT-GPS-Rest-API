using AutoMapper;
using Trails.Models.DTOs;

namespace Trails.Models.Profiles
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<Device, DeviceDtoGet>();
            CreateMap<DeviceDtoPost, Device>();
            CreateMap<DeviceDtoPut, Device>();
        }
    }
}
