using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Eskom;
using Services.Eskom;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
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
        [SwaggerOperation(Summary = "Get all Suburbs as per Eskom site")]
        public async Task<IActionResult> GetSuburbList(int provinceId, int municipalityId)
        {
            var res = await _eskomService.GetSuburbListByMunicipality(provinceId, municipalityId);
            return Ok(res);
        }

        [HttpGet("FindSuburb")]
        [ProducesResponseType(typeof(IEnumerable<SuburbData>), 200)]
        [SwaggerOperation(Summary = "Search for the supplied suburb name on the Eskom Site")]
        public async Task<IActionResult> FindSuburb(string suburbname, int? municipalityId)
        {
            var res = await _eskomService.FindSuburb(suburbname, municipalityId);
            return Ok(res);
        }


        [HttpGet("GetSchedule")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [SwaggerOperation(Summary = "Get the schedule for the next x days")]
        public async Task<IActionResult> GetSchedule(int municipalityId, int blockId, int days, int stage)
        {
            var res = await _eskomService.GetSchedule(municipalityId, blockId, days, stage);
            return Ok(res);
        }
    }

}
