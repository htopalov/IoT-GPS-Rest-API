using System;
using System.Collections.Generic;
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
    public class PositionDataController : ControllerBase
    {
        private readonly TrailsContext context;
        private readonly IMapper mapper;

        public PositionDataController(TrailsContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //api/PositionData
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PositionDataDtoGet>>> GetAllPositionDataAsync()
        {
            var positionData = await context.PositionData.ToListAsync();
            return Ok(mapper.Map<IEnumerable<PositionDataDtoGet>>(positionData));
        }

        //api/PositionData
        [HttpPost]
        [AuthKey]
        public async Task<ActionResult<PositionDataDtoPost>> PostPositionDataAsync(PositionDataDtoPost positionDataDtoPost)
        {
            var positionDataModel = mapper.Map<PositionData>(positionDataDtoPost);
            var device = await context.Devices.FindAsync(positionDataDtoPost.DeviceId);

            if (device == null)
            {
                return NotFound(Messages.DeviceNotExist);
            }

            positionDataModel.Timestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            await context.PositionData.AddAsync(positionDataModel);
            await context.SaveChangesAsync();
            return Ok();
            //no need to return create at route with route and object to device
            //it only needs to know the status code for success
            //not exactly restful compliant but no practical need for it
        }
    }
}
