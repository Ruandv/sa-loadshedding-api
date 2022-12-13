using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using EskomCalendarApi.Enums;
using System;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace EskomCalendarApi.Controllers.Logging
{
	[ApiController]
	[Route("api/[controller]/")]
	public class LoggingController : ControllerBase
	{
		 
		FirebaseApp app = FirebaseApp.Create(new AppOptions()
		{
			Credential = GoogleCredential.GetApplicationDefault(),
		});

		private ILogger<LoggingController> _logger;

		public LoggingController(ILogger<LoggingController> logger)
		{
			_logger = logger;
			_logger.LogInformation("Init LoggingController");
		}

		[HttpPost()]
		[SwaggerOperation(Summary = "Logs the message that was send")]
		[ProducesResponseType(typeof(string), 200)]
		public async Task<IActionResult> LogWarning(string message)
		{
		 
			return Ok( );
		}
	}
}
