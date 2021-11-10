using System.Collections.Generic;
using System.Threading.Tasks;
using Trails.Domain.Models;

namespace Trails.Services.DeviceService
{
    public interface IDeviceService
    {
        Task<List<Device>> GetDevicesAsync();

        Task<List<PositionData>> GetAllPositionDataForDeviceAsync(string deviceId);

        Task<Device> GetDeviceByIdAsync(string deviceId);

        Task<bool> CreateDeviceAsync(Device device);

        Task<bool> UpdateDeviceAsync(Device deviceToUpdate);

        Task<bool> DeleteDeviceAsync(string deviceId);

    }
}
