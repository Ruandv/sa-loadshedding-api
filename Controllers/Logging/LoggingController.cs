using EskomCalendarApi.Enums;
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
        public async Task<IActionResult> LogWarning(ExtensionMessageType messageType, string message)
        {
            switch (messageType)
            {
                case ExtensionMessageType.INSTALLED:
                    _logService.Installed(message);
                    break;
                case ExtensionMessageType.UNINSTALLED:
                    _logService.UnInstalled(message);
                    break;
                case ExtensionMessageType.SUBURBADDED:

                    break;
                case ExtensionMessageType.SUBURBVIEWED:

                    break;
                case ExtensionMessageType.SUBURBREMOVED:

                    break;
                case ExtensionMessageType.DAYSCHANGED:

                    break;
                case ExtensionMessageType.SEARCHED:

                    break;

            }

            return Ok();
        }
    }
}
