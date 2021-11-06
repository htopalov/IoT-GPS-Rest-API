using AutoMapper;
using Trails.Domain.DTOs.Device;
using Trails.Domain.Models;

namespace Trails.Domain.Profiles
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
