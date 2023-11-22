using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Eskom;
using Models.Esp;
using Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers.Eskom
{
    [ApiController]
    [Route("api/[controller]/")]
    public class EskomController : ControllerBase
    {
        private readonly IEskomService _eskomService;
        private readonly IEspService _espService;
        private readonly ILogger<EskomController> _logger;

        public EskomController(ILogger<EskomController> logger, IEskomService eskomService, IEspService espService)
        {
            _logger = logger;
            _logger.LogInformation("Init EskomController");
            _eskomService = eskomService;
            _espService = espService;
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

        [HttpGet("GetStatus")]
        [ProducesResponseType(typeof(int), 200)]
        [SwaggerOperation(Summary = "Get the current stage")]
        public async Task<IActionResult> GetStatus()
        {
            var res = await _eskomService.GetStatus<int>(string.Empty);
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

        [HttpGet("GetESPSchedule")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [SwaggerOperation(Summary = "Get the schedule for the next x days but uses the ESP Status")]
        public async Task<IActionResult> GetSchedule(int municipalityId = 166, int blockId = 12, int days = 1)
        {
            var headerList = Request.Headers;
            //var authorizationField = headerList.Get("Authorization");

            var res = await _eskomService.GetSchedule(municipalityId, blockId, days, 8);

            var espStatus = await _espService.GetStatus();

            //find all the dates
            var dates = res.Select(x => x.DayOfMonth).ToList();

            // if there are next stages get the first one
            NextStage endstg = espStatus.status.eskom.next_stages.Count > 0 ? espStatus.status.eskom.next_stages[0] : null;
            var stage = int.Parse(espStatus.status.eskom.stage);
            var statDate = DateTime.Now;
            var DatTimeToday = res.Where(x => x.Stage <= stage && x.DayOfMonth.AddTicks(x.Start.Ticks).AddMinutes(-31) < statDate).ToList();
            var st = new List<ScheduleDto>();

            if (!espStatus.status.eskom.next_stages.ToList().Any())
            {
                st = res.Where(x => x.Stage <= stage).ToList();
            }
            else
            {
                foreach (var item in espStatus.status.eskom.next_stages.ToList())
                {
                    if (int.Parse(item.stage) != stage)
                    {
                        //get everything where date = new date and stage == old stage;
                        st = res.Where(x => x.Stage <= stage && x.DayOfMonth.Day == item.stage_start_timestamp.Day && x.DayOfMonth.AddTicks(x.Start.Ticks) < item.stage_start_timestamp).ToList();
                        // remove those already in the array
                        DatTimeToday.ForEach(n =>
                        {
                            var r = st.Find(x => x == n);
                            st.Remove(r);
                        });

                        DatTimeToday.AddRange(st);
                        stage = int.Parse(item.stage);
                    }
                    st = res.Where(x => x.Stage <= stage && x.DayOfMonth.Day == item.stage_start_timestamp.Day && x.DayOfMonth.AddTicks(x.Start.Ticks) >= item.stage_start_timestamp).ToList();
                }
            }
            DatTimeToday.AddRange(st);

            return Ok(DatTimeToday);
        }

    }

}
