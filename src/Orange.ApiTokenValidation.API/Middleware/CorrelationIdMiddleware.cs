using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Orange.ApiTokenValidation.API.Middleware
{
    /// <summary>
    /// Correlation id middleware
    /// </summary>
    public class CorrelationIdMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        public const string CorrelationHeaderName = "x-correlation-id";
        private const string CorrelationIdScope = "CorrelationID";

        /// <summary>
        /// .ctor
        /// </summary>
        public CorrelationIdMiddleware(ILogger<CorrelationIdMiddleware> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Add correlation id
        /// </summary>
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!context.Request.Headers.TryGetValue(CorrelationHeaderName, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }

            using (_logger.BeginScope(new Dictionary<string, object> {[CorrelationIdScope] = correlationId}))
            {
                //var requestIdFeature = context.Features.Get<IHttpRequestIdentifierFeature>();
                //if (requestIdFeature?.TraceIdentifier != null)
                //{
                //     requestIdFeature.TraceIdentifier;
                //}

                context.Response.Headers[CorrelationHeaderName] = correlationId;

                return next(context);
            }
        }
    }
}
