using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace Orange.ApiTokenValidation.API.Middleware
{
    /// <summary>
    /// Write request metrics
    /// </summary>
    public class RequestWriterMiddleware : IMiddleware
    {
        private readonly ILogger _logger;

        /// <summary>
        /// .ctor
        /// </summary>
        public RequestWriterMiddleware(ILogger<RequestWriterMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            var path = httpContext.Request.Path.Value;
            var method = httpContext.Request.Method;

            var counter = Metrics.CreateCounter("request_total", "HTTP Requests Total", 
                new CounterConfiguration
                {
                    LabelNames = new[] { "path", "method", "status" }
                });

            try
            {
                await next.Invoke(httpContext);
            }
            finally
            {
                var statusCode = httpContext.Response.StatusCode;
                counter.Labels(path, method, statusCode.ToString()).Inc();
            }
        }
    }
}
