using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Orange.ApiTokenValidation.API.Filters
{
    /// <summary>
    /// Correlation id middleware
    /// </summary>
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private const string Header = "x-correlation-id";
        private const string CorrelationIdScope = "CorrelationID";

        /// <summary>
        /// .ctor
        /// </summary>
        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Add correlation id
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(Header, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString(); 
            }

            using (_logger.BeginScope(new Dictionary<string, object> { [CorrelationIdScope] = correlationId }))
            {
                //var requestIdFeature = context.Features.Get<IHttpRequestIdentifierFeature>();
                //if (requestIdFeature?.TraceIdentifier != null)
                //{
                //     requestIdFeature.TraceIdentifier;
                //}

                context.Response.Headers[Header] = correlationId;

                await _next(context);
            }
        }
    }
}
