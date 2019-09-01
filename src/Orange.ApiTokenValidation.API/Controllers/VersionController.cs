using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;

namespace Orange.ApiTokenValidation.API.Controllers
{
    /// <summary>
    /// Version controller
    /// </summary>
    [RequireHttps]
    [Route("[controller]")]
    public class VersionController : Controller
    {
        private readonly ILogger _logger;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="logger"></param>
        public VersionController(ILogger<VersionController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Application version
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetVersion()
        {
            using (_logger.BeginScope("CorrelationID"))
            {
                _logger.LogInformation("Version request");

                var version = PlatformServices.Default.Application.ApplicationVersion;

                return Ok(version);
            }
        }
    }
}
