using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Orange.ApiTokenValidation.Application.Interfaces;

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
            httpContext.Request.Headers.TryGetValue(CorrelationIdMiddleware.CorrelationHeaderName,
                                                    out var correlationId);
            var sw = Stopwatch.StartNew();
            try
            {
                await next.Invoke(httpContext);
            }
            finally
            {
                sw.Stop();
                var statusCode = httpContext.Response.StatusCode;
                _measurer.RequestMetric(path, method, statusCode, correlationId);
                
                _logger.LogDebug($"{method} {path} {statusCode} {correlationId} {sw.ElapsedMilliseconds}");
            }
        }
    }
}
