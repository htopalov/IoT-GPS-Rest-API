using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DeviceSecurity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trails.Models;
using Trails.Models.Context;
using Trails.Models.DTOs;
using Utilities;

namespace Trails.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly TrailsContext context;
        private readonly IMapper mapper;

        public DeviceController(TrailsContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //api/Device
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceDtoGet>>> GetDevicesAsync()
        {
            var devices = await context.Devices.ToListAsync();
            return Ok(mapper.Map<IEnumerable<DeviceDtoGet>>(devices));
        }

        //api/Device/111111111111111
        [HttpGet("{id}", Name = "GetDeviceAsync")]
        public async Task<ActionResult<DeviceDtoGet>> GetDeviceAsync(string id)
        {
            var device = await context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound(Messages.DeviceNotExist);
            }

            return Ok(mapper.Map<DeviceDtoGet>(device));
        }

        //api/Device/PositionData/1
        [HttpGet("position-data-list/{id}")]
        public async Task<ActionResult<IEnumerable<PositionDataDtoGet>>> GetListOfPositionDataForDeviceAsync(string id)
        {
            var device = await context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound(Messages.DeviceNotExist);
            }

            var positionData = await context.PositionData
                .Where(x => x.DeviceId == id)
                .ToListAsync();
            return Ok(mapper.Map<IEnumerable<PositionDataDtoGet>>(positionData));
        }

        //api/Device/111111111111111
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeviceAsync(string id, DeviceDtoPut deviceDtoPut)
        {
            var deviceFromDb = await context.Devices.FindAsync(id);
            if (deviceFromDb == null)
            {
                return NotFound(Messages.DeviceNotExist);
            }

            mapper.Map(deviceDtoPut, deviceFromDb);
            await context.SaveChangesAsync();
            return NoContent();
        }

        //api/Device
        [HttpPost]
        public async Task<ActionResult<DeviceDtoPost>> PostDeviceAsync(DeviceDtoPost deviceDtoPost)
        {
            var deviceModel = mapper.Map<Device>(deviceDtoPost);
            var currentDevice = await context.Devices.FindAsync(deviceModel.DeviceId);
            if (currentDevice !=null)
            {
                return Conflict(Messages.DeviceExists);
            }

            string accessKey = AccessKeyGenerator.GenerateRandomKey();
            string accessKeySalt = AccessKeyGenerator.GenerateSalt();
            string hashedKey = HashGenerator.ComputeHash(Encoding.UTF8.GetBytes(accessKey),
                Encoding.UTF8.GetBytes(accessKeySalt));
            string encryptedDeviceKey = CryptoGenerator.Encrypt(accessKey);

            deviceModel.AccessKey = hashedKey;
            deviceModel.Salt = accessKeySalt;

            await context.Devices.AddAsync(deviceModel);
            await context.SaveChangesAsync();
            return CreatedAtRoute(nameof(GetDeviceAsync), new {Id = deviceDtoPost.DeviceId},
                new {deviceDtoPost, EncryptedKey = encryptedDeviceKey});
        }

        //api/Device/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceAsync(string id)
        {
            var device = await context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound(Messages.DeviceNotExist);
            }

            context.Devices.Remove(device);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
