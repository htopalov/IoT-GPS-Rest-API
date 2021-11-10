using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Trails.Data;
using Trails.Domain.Models;

namespace Trails.Services.DeviceService
{
    public class DeviceService : IDeviceService
    {
        private readonly DataContext dataContext;

        public DeviceService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<Device>> GetDevicesAsync()
        {
            return await dataContext.Devices.ToListAsync();
        }

        public async Task<List<PositionData>> GetAllPositionDataForDeviceAsync(string deviceId)
        {
            var device = await dataContext.Devices.SingleOrDefaultAsync(x => x.DeviceId == deviceId);
            if (device == null)
            {
                return null;
            }

            return device.PositionData.ToList();
        }

        public async Task<Device> GetDeviceByIdAsync(string deviceId)
        {
            return await dataContext.Devices.SingleOrDefaultAsync(x => x.DeviceId == deviceId);
        }

        public async Task<bool> CreateDeviceAsync(Device device)
        {
            await dataContext.Devices.AddAsync(device);
            var created = await dataContext.SaveChangesAsync();
            return created > 0;

        }

        public async Task<bool> UpdateDeviceAsync(Device deviceToUpdate)
        {
            if (deviceToUpdate == null)
            {
                return false;
            }
            dataContext.Devices.Update(deviceToUpdate);
            var updated = await dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeleteDeviceAsync(string deviceId)
        {
            var device = await GetDeviceByIdAsync(deviceId);
            if (device == null)
            {
                return false;
            }
            dataContext.Devices.Remove(device);
            var deleted = await dataContext.SaveChangesAsync();
            return deleted > 0;
        }
    }
}
