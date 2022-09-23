using EskomCalendarApi.Models.Calendar;
using EskomCalendarApi.Services.Calendar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EskomCalendarApi.Controllers.Eskom
{
    [ApiController]
    [Route("api/[controller]/")]
    public class EskomController : ControllerBase
    {
        private readonly IEskomService _eskomService;
        private readonly ILogger<EskomController> _logger;

        public EskomController(ILogger<EskomController> logger, IEskomService eskomService)
        {
            _logger = logger;
            _logger.LogInformation("Init CalendarController");
            _eskomService = eskomService;
        }

        [HttpGet("GetProvinceList")]
        [ProducesResponseType(typeof(IEnumerable<Province>), 200)]
        [SwaggerOperation(Summary = "Get all the provinces as per Eskom site")]
        public async Task<IActionResult> GetProvinceList()
        {
            var res = await _eskomService.GetProvinces();
            return Ok(res);
        }

        [HttpGet("GetMunicipalityList")]
        [ProducesResponseType(typeof(IEnumerable<Municipality>), 200)]
        [SwaggerOperation(Summary = "Get all municipalities as per Eskom site")]
        public async Task<IActionResult> GetMunicipalityList(int provinceId)
        {
            var res = await _eskomService.GetMunicipalities(provinceId);
            return Ok(res);
        }

        [HttpGet("GetSuburbList")]
        [ProducesResponseType(typeof(IEnumerable<SuburbData>), 200)]
        [SwaggerOperation(Summary = "Get all municipalities as per Eskom site")]
        public async Task<IActionResult> GetSuburbList(int municipalityId, int? blockId)
        {
            if (municipalityId == 166 || municipalityId == 167)
            {
                var res = await _eskomService.GetSuburbsByMunicipality(municipalityId, blockId);
                return Ok(res);
            }
            else
            {
                return BadRequest("The only supported municipalities are Tshwane (167) and city-power (166)");
            }
        }
    }

}
