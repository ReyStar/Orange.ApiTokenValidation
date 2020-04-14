using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orange.ApiTokenValidation.Domain.Interfaces;
using Prometheus;

namespace Orange.ApiTokenValidation.Metrics
{
    public static class DependenciesRegistrator
    {
        public static void RegisterMetricsDependencies(this IServiceCollection collection, HostBuilderContext hostBuilderContext)
        {
            collection.AddSingleton<IMeasurer, Measurer>();

            collection.AddHostedService<HeartbeatService>();
        }

        public static void ConfigureMeasure(this IApplicationBuilder app)
        {
            app.UseMetricServer();
            
            app.UseHttpMetrics();
        }
    }
}
