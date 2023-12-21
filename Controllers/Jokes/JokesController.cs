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
            _logger.LogInformation("Generating a JOKE!!!");
            var res = await _jokesService.GetJoke();
            return Ok(res);
        }

        [HttpPost("GetImage")]
        [ProducesResponseType(typeof(int), 200)]
        [SwaggerOperation(Summary = "Gets an image from the url supplied")]
        public async Task<IActionResult> GetImage([FromBody] MyImageData imgData)
        {
            var res = await _jokesService.GetImage(url: imgData.url);
            return Ok(res);
        }
    }
}
