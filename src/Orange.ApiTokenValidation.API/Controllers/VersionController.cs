using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Orange.ApiTokenValidation.API.Controllers
{
    /// <summary>
    /// Version controller
    /// </summary>
    [RequireHttps]
    [Route("[controller]")]
    public class VersionController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            var version = Assembly.GetEntryAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;

            return Ok(version);
        }
    }
}
