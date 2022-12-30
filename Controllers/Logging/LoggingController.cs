using EskomCalendarApi.Enums;
using EskomCalendarApi.Models.Logging;
using EskomCalendarApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
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
                    obj.UserToken = mi.userToken;
                    _logService.Installed(obj);
                    break;
                case ExtensionMessageType.SUBURBADDED:
                    var subItemA = System.Text.Json.JsonSerializer.Deserialize<SuburbItem>(dta);
                    subItemA.UserToken = mi.userToken;
                    _logService.SuburbAdded(subItemA);
                    break;
                case ExtensionMessageType.SUBURBVIEWED:
                    var subItemV = System.Text.Json.JsonSerializer.Deserialize<SuburbItem>(dta);
                    subItemV.UserToken = mi.userToken;
                    _logService.SuburbViewed(subItemV);
                    break;
                case ExtensionMessageType.SUBURBREMOVED:
                    var subItemRem = System.Text.Json.JsonSerializer.Deserialize<SuburbItem>(dta);
                    subItemRem.UserToken = mi.userToken;
                    _logService.SuburbAdded(subItemRem);
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
            _logService.UnInstalled(new InstalledItem() { Message = "Uninstalled", UserToken = userToken });

            return Redirect("https://hmpg.net/");
        }
    }

    public class MessageInformation
    {
        public ExtensionMessageType messageType { get; set; }
        public string userToken { get; set; }
        public string message { get; set; }
    }
}
