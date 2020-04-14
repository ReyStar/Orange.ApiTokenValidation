using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Serilog;

namespace Orange.ApiTokenValidation.Shell
{
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Register Serilog
        /// https://dotnet-cookbook.cfapps.io/core/scoped-logging-with-serilog/
        /// https://andrewlock.net/adding-serilog-to-the-asp-net-core-generic-host/
        /// </summary>
        public static IHostBuilder RegisterLogger(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureLogging(logging => { logging.ClearProviders(); })
                              .UseSerilog((context, configuration) =>
                              {
                                  configuration.ReadFrom.Configuration(context.Configuration);
                                  configuration.Enrich.WithDynamicProperty("MemoryUsage", () => DebugInfoProvider.GetMemoryUsage(3))
                                               .Enrich.WithDynamicProperty("ThreadMemoryUsage", () => DebugInfoProvider.GetThreadMemoryUsage(3))
                                               .Enrich.WithDynamicProperty("GC", () => DebugInfoProvider.GetGenerations())
                                               .Enrich.WithProperty("Version", PlatformServices.Default.Application.ApplicationVersion);
                              }, writeToProviders: true);
        }
    }
}