﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Esp;
using Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Controllers.ESP
{
    [ApiController]
    [Route("api/[controller]/")]
    public class EspController : ControllerBase
    {
        private readonly IEspService _espService;
        private readonly ILogger<EspController> _logger;
        public EspController(ILogger<EspController> logger, IEspService espService)
        {
            _logger = logger;
            _espService =  espService;
        }

        [HttpGet("Status")]
        [ProducesResponseType(typeof(StatusObject), 200)]
        [SwaggerOperation(Summary = "Get the current status on a national level")]
        public async Task<IActionResult> GetStatus()
        {
            var res = await _espService.GetStatus();
            return Ok(res);
        }
        [HttpGet("areas_search")]
        [ProducesResponseType(typeof(AreasObject), 200)]
        [SwaggerOperation(Summary = "Search area based on text")]
        public async Task<IActionResult> AreasSearch(string text)
        {
            Request.Headers.TryGetValue("token", out var token);
            var res = await _espService.AreasSearch(token, text);
            return Ok(res);
        }

        [HttpGet("area_information")]
        [SwaggerOperation(Summary = "This single request has everything you need to monitor upcoming loadshedding events for the chosen suburb.")]
        public async Task<IActionResult> AreaInformation(string id)
        {
            Request.Headers.TryGetValue("token", out var token);
            var res = await _espService.AreaInformation(token, id);
            return Ok(res);
        }

        [HttpGet("api_allowance")]
        [SwaggerOperation(Summary = "This single request has everything you need to monitor upcoming loadshedding events for the chosen suburb.")]
        public async Task<IActionResult> api_allowance(string id)
        {
            Request.Headers.TryGetValue("token", out var token);
            var res = await _espService.ApiAllowance(token, id);
            return Ok(res);
        }
    }

}
