using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trails.Domain.Models;

namespace Trails.Services.PositionDataService
{
    public interface IPositionDataService
    {
        Task<PositionData> GetPositionDataByIdAsync(Guid dataId);

        Task<List<PositionData>> GetAllPositionDataAsync();

        Task<bool> CreatePositionDataAsync(PositionData positionData);
    }
}
