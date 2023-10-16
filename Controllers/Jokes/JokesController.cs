using Controllers.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Controllers.Jokes
{
    [ApiController]
    [Route("api/[controller]")]
    public class JokesController : ControllerBase
    {
        private ILogger<JokesController> _logger;
        private ILoggingService _logService;
        private IJokesService _jokesService;

        public JokesController(ILogger<JokesController> logger, ILoggingService logService, IJokesService jokes)
        {
            _logger = logger;
            _logger.LogInformation("Init JokesController");
            _logService = logService;
            _jokesService = jokes;
        }

        [HttpGet("GetJoke")]
        [ProducesResponseType(typeof(int), 200)]
        [SwaggerOperation(Summary = "Gets one of the jokes")]
        public async Task<IActionResult> GetStatus()
        {
            var res = await _jokesService.GetJoke();
            return Ok(res);
        }
    }
}
