using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Trails.Data;
using Trails.Domain.Models;

namespace Trails.Services.PositionDataService
{
    public class PositionDataService : IPositionDataService
    {
        private readonly DataContext dataContext;

        public PositionDataService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<PositionData> GetPositionDataByIdAsync(Guid dataId)
        {
            return await dataContext.PositionData.SingleOrDefaultAsync(x => x.DataId == dataId);
        }

        public async Task<List<PositionData>> GetAllPositionDataAsync()
        {
            return await dataContext.PositionData.ToListAsync();
        }

        public async Task<bool> CreatePositionDataAsync(PositionData positionData)
        {
            var deviceExists = await dataContext.Devices.SingleOrDefaultAsync(d => d.DeviceId == positionData.DeviceId);
            if (deviceExists == null)
            {
                return false;
            }
            positionData.Timestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            await dataContext.PositionData.AddAsync(positionData);
            var created = await dataContext.SaveChangesAsync();
            return created > 0;
        }
    }
}
