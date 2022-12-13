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

        [HttpGet("FindSuburb")]
        [ProducesResponseType(typeof(IEnumerable<SuburbData>), 200)]
        [SwaggerOperation(Summary = "Search for the supplied suburb name on the Eskom Site")]
        public async Task<IActionResult> FindSuburb(string suburbname)
        {
                var res = await _eskomService.FindSuburb(suburbname);
                return Ok(res);
        }


        [HttpGet("GetStatus")]
        [ProducesResponseType(typeof(int), 200)]
        [SwaggerOperation(Summary = "Get the current Loadshedding stage")]
        public async Task<IActionResult> GetStatus()
        {
            var res = await _eskomService.GetStatus();
            return Ok(res);
        }
    }

}
