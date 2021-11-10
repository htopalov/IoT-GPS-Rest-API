using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Trails.Domain.DTOs.Ver1.PositionDataDTOs.Requests;
using Trails.Domain.DTOs.Ver1.PositionDataDTOs.Responses;
using Trails.Domain.Models;
using Trails.Routes.Ver1;
using Trails.Services.PositionDataService;

namespace Trails.Web.Controllers.Ver1
{
    public class PositionDataController : Controller
    {
        private readonly IPositionDataService positionDataService;
        private readonly IMapper mapper;

        public PositionDataController(IPositionDataService positionDataService, IMapper mapper)
        {
            this.positionDataService = positionDataService;
            this.mapper = mapper;
        }

        [HttpGet(PositionDataRoutes.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(mapper.Map<IEnumerable<PositionDataGet>>(await positionDataService.GetAllPositionDataAsync()));
        }

        [HttpGet(PositionDataRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid dataId)
        {
            var positionData = await positionDataService.GetPositionDataByIdAsync(dataId);

            if (positionData == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<PositionDataGet>(positionData));
        }

        [HttpPost(PositionDataRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] PositionDataPost dataToPost)
        {
            var positionData = new PositionData();
            mapper.Map(dataToPost, positionData);
            var created = await positionDataService.CreatePositionDataAsync(positionData);
            if (!created)
            {
                return BadRequest();
            }

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + PositionDataRoutes.Get.Replace("{dataId}", positionData.DataId.ToString());
            return Created(locationUrl, dataToPost);
        }
    }
}
