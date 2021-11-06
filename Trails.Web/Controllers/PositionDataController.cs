using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trails.Data;
using Trails.Domain.DTOs.PositionData;
using Trails.Domain.Models;

namespace Trails.Web.Controllers
{
    [Route("api/position-data")]
    [ApiController]
    public class PositionDataController : ControllerBase
    {
        private readonly TrailsDataContext context;
        private readonly IMapper mapper;

        public PositionDataController(TrailsDataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPositionDataAsync()
        {
            var positionData = await context.PositionData.ToListAsync();
            return Ok(mapper.Map<IEnumerable<PositionDataDtoGet>>(positionData));
        }

        [HttpPost]
        public async Task<IActionResult> PostPositionDataAsync(PositionDataDtoPost positionDataDtoPost)
        {
            var positionDataModel = mapper.Map<PositionData>(positionDataDtoPost);

            if (await context.Devices.FindAsync(positionDataDtoPost.DeviceId) == null)
            {
                return NotFound();
            }

            positionDataModel.Timestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            await context.PositionData.AddAsync(positionDataModel);
            await context.SaveChangesAsync();
            return Ok();
            //no need to return create at route with route and object to gps device
            //it only needs to know the status code for success
            //not exactly restful compliant but no practical need for it
        }
    }
}

