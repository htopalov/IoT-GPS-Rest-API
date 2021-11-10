using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Trails.Domain.DTOs.Ver1.DeviceDTOs.Requests;
using Trails.Domain.DTOs.Ver1.DeviceDTOs.Responses;
using Trails.Domain.DTOs.Ver1.PositionDataDTOs.Responses;
using Trails.Domain.Models;
using Trails.Routes.Ver1;
using Trails.Services.DeviceService;

namespace Trails.Web.Controllers.Ver1
{
    [ApiController]
    public class DeviceController : Controller
    {
        private readonly IDeviceService deviceService;
        private readonly IMapper mapper;

        public DeviceController(IDeviceService deviceService, IMapper mapper)
        {
            this.deviceService = deviceService;
            this.mapper = mapper;
        }

        [HttpGet(DeviceRoutes.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(mapper.Map<IEnumerable<DeviceGet>>(await deviceService.GetDevicesAsync()));
        }

        [HttpGet(DeviceRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute]string deviceId)
        {
            var device = await deviceService.GetDeviceByIdAsync(deviceId);

            if (device == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<DeviceGet>(device));
        }

        [HttpGet(DeviceRoutes.GetAllDeviceData)]
        public async Task<IActionResult> GetAllDeviceData([FromRoute] string deviceId)
        {
            var data = await deviceService.GetAllPositionDataForDeviceAsync(deviceId);
            if (data == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<IEnumerable<PositionDataGet>>(data));
        }

        [HttpPost(DeviceRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] DevicePost deviceToPost)
        {
            var device = new Device();
            mapper.Map(deviceToPost, device);
            var created = await deviceService.CreateDeviceAsync(device);
            if (!created)
            {
                return BadRequest();
            }

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + DeviceRoutes.Get.Replace("{deviceId}", device.DeviceId);
            return Created(locationUrl,deviceToPost);
        }


        [HttpPut(DeviceRoutes.Update)]
        public async Task<IActionResult> Update([FromRoute] string deviceId, [FromBody]DevicePut deviceUpdate)
        {
            var device = await deviceService.GetDeviceByIdAsync(deviceId);

            mapper.Map(deviceUpdate, device);
            var updated = await deviceService.UpdateDeviceAsync(device);
            if (updated)
            {
                return Ok(deviceUpdate);
            }

            return NotFound();
        }

        [HttpDelete(DeviceRoutes.Delete)]
        public async Task<IActionResult> Delete([FromRoute] string deviceId)
        {
            var deleted = await deviceService.DeleteDeviceAsync(deviceId);
            if (deleted)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
