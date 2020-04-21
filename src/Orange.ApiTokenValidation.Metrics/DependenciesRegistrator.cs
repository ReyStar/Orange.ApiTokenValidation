using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Orange.ApiTokenValidation.Application.Interfaces;
using Orange.ApiTokenValidation.Common;
using Prometheus;

namespace Orange.ApiTokenValidation.Metrics
{
    public static class DependenciesRegistrator
    {
        public static void RegisterMetricsDependencies(this IServiceCollection collection, HostBuilderContext hostBuilderContext)
        {
            collection.AddSingleton<IMeasurer, Measurer>();

            collection.Configure<MeasurerConfiguration>(hostBuilderContext.Configuration.GetSection(nameof(MeasurerConfiguration)));
        }

        public static void ConfigureMeasure(this IApplicationBuilder app)
        {
            var measurerConfiguration = app.ApplicationServices.GetService<IOptions<MeasurerConfiguration>>().Value;
            
            app.UseMetricServer(measurerConfiguration.PullMetricsUrl);
            app.UseHttpMetrics();
        }
    }
}
