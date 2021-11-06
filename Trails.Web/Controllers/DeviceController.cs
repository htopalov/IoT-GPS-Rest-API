using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trails.Data;
using Trails.Domain.DTOs.Device;
using Trails.Domain.DTOs.PositionData;
using Trails.Domain.Models;

namespace Trails.Web.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly TrailsDataContext context;
        private readonly IMapper mapper;

        public DeviceController(TrailsDataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetDevicesAsync()
        {
            var devices = await context.Devices.ToListAsync();
            return Ok(mapper.Map<IEnumerable<DeviceDtoGet>>(devices));
        }

        [HttpGet("{id}", Name = "GetDeviceAsync")]
        public async Task<IActionResult> GetDeviceAsync(string id)
        {
            var device = await context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<DeviceDtoGet>(device));
        }

        [HttpGet("position-data-list/{id}")]
        public async Task<IActionResult> GetListOfPositionDataForDeviceAsync(string id)
        {
            var device = await context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            var positionData = await context.PositionData
                .Where(x => x.DeviceId == id)
                .ToListAsync();
            return Ok(mapper.Map<IEnumerable<PositionDataDtoGet>>(positionData));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeviceAsync(string id, DeviceDtoPut deviceDtoPut)
        {
            var deviceFromDb = await context.Devices.FindAsync(id);
            if (deviceFromDb == null)
            {
                return NotFound();
            }

            mapper.Map(deviceDtoPut, deviceFromDb);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostDeviceAsync(DeviceDtoPost deviceDtoPost)
        {
            var deviceModel = mapper.Map<Device>(deviceDtoPost);
            if (await context.Devices.FindAsync(deviceModel.DeviceId) != null)
            {
                return BadRequest();
            }

            await context.Devices.AddAsync(deviceModel);
            await context.SaveChangesAsync();
            return CreatedAtRoute(nameof(GetDeviceAsync), new { Id = deviceDtoPost.DeviceId }, new { deviceDtoPost});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceAsync(string id)
        {
            var device = await context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            context.Devices.Remove(device);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
