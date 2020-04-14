using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Orange.ApiTokenValidation.Domain.Interfaces;

namespace Orange.ApiTokenValidation.API.Middleware
{
    /// <summary>
    /// Write request metrics
    /// </summary>
    public class RequestWriterMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        private readonly IMeasurer _measurer;

        /// <summary>
        /// .ctor
        /// </summary>
        public RequestWriterMiddleware(ILogger<RequestWriterMiddleware> logger, IMeasurer measurer)
        {
            _logger = logger;
            _measurer = measurer;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            var path = httpContext.Request.Path.Value;
            var method = httpContext.Request.Method;

            try
            {
                await next.Invoke(httpContext);
            }
            finally
            {
                var statusCode = httpContext.Response.StatusCode;
                _measurer.RequestMetric(path, method, statusCode);
            }
        }
    }
}
