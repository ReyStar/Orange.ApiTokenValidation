using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Orange.ApiTokenValidation.API.Attributes;

namespace Orange.ApiTokenValidation.API.Controllers
{
    /// <summary>
    /// Version controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [RequireHttps]
    [NullModelValidation]
    [ValidateModel]
    [AdvertiseApiVersions]//for ignore api version require
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
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetVersion()
        {
            var version = PlatformServices.Default.Application.ApplicationVersion;

            return Ok(version);
        }
    }
}
