using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Orange.ApiTokenValidation.Domain.Exceptions;

namespace Orange.ApiTokenValidation.API.Filters
{
    /// <summary>
    /// Filter fro catch known domain exception
    /// </summary>
    public class KnownExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        /// <summary>
        /// .ctor
        /// </summary>
        public KnownExceptionFilter(ILogger<KnownExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is TokenValidationException ex)
            {
                _logger.LogDebug($"Validation error: {ex.Message}");

                context.ExceptionHandled = true;
                context.Result = new StatusCodeResult(StatusCodes.Status400BadRequest);
                context.HttpContext.Response.Headers.Add("reason-phrase", ex?.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
