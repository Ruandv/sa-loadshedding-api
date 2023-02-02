﻿using EskomCalendarApi.Enums;
using EskomCalendarApi.Models.Logging;
using EskomCalendarApi.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace EskomCalendarApi.Controllers.Logging
{
    [ApiController]
    [Route("api/[controller]/")]
    public class LoggingController : ControllerBase
    {
        private ILogger<LoggingController> _logger;
        private LoggingService _logService;


        public LoggingController(ILogger<LoggingController> logger, LoggingService logService)
        {
            _logger = logger;
            _logger.LogInformation("Init LoggingController");
            _logService = logService;
        }

        [HttpPost()]
        [SwaggerOperation(Summary = "Logs the message that was send")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> LogWarning([FromBody] MessageInformation mi)
        {

            var dta = mi.message;
            switch (mi.messageType)
            {
                case ExtensionMessageType.INSTALLED:
                    var obj = System.Text.Json.JsonSerializer.Deserialize<InstalledItem>(dta);
                    obj.ActionDate = DateTime.Now;
                    _logService.Installed(obj);
                    break;
                case ExtensionMessageType.UPDATED:
                    var obj2 = System.Text.Json.JsonSerializer.Deserialize<InstalledItem>(dta);
                    obj2.ActionDate = DateTime.Now;
                    _logService.Installed(obj2);
                    break;
                case ExtensionMessageType.SUBURBADDED:
                    var subItemA = System.Text.Json.JsonSerializer.Deserialize<SuburbItem>(dta);
                    subItemA.ActionDate = DateTime.Now;
                    _logService.SuburbAdded(subItemA);
                    break;
                case ExtensionMessageType.SUBURBVIEWED:
                    var subItemV = System.Text.Json.JsonSerializer.Deserialize<SuburbItem>(dta);
                    subItemV.ActionDate = DateTime.Now;
                    if (subItemV.SuburbName != "newSub")
                    {
                        _logService.SuburbViewed(subItemV);
                    }
                    break;
                case ExtensionMessageType.SUBURBREMOVED:
                    var subItemRem = System.Text.Json.JsonSerializer.Deserialize<SuburbItem>(dta);
                    subItemRem.ActionDate = DateTime.Now;
                    _logService.SuburbRemoved(subItemRem);
                    break;
                case ExtensionMessageType.DAYSCHANGED:

                    break;
                case ExtensionMessageType.SEARCHED:

                    break;

            }

            return Ok();
        }

        [HttpGet()]
        [SwaggerOperation(Summary = "Logs the message that was send")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> Uninstalled(string userToken)
        {
            _logService.UnInstalled(new InstalledItem() { ActionDate = DateTime.Now, Message = "UNINSTALLED", UserToken = userToken });

            return Redirect("https://hmpg.net/");
        }
    }

    public class MessageInformation
    {
        public ExtensionMessageType messageType { get; set; }
        public string message { get; set; }
    }
}
