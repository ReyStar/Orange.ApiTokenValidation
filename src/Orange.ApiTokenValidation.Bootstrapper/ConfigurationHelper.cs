using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Serilog;

namespace Orange.ApiTokenValidation.Bootstrapper
{
    public static class ConfigurationHelper
    {
        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> RegisterConfiguration<T>(this ContainerBuilder containerBuilder)
        {
            return containerBuilder.Register(context =>
             {
                 var configuration = context.Resolve<IConfiguration>();
                 return configuration.GetSection(typeof(T).Name).Get<T>();
             }).As<T>();
        }

        /// <summary>
        /// https://dotnet-cookbook.cfapps.io/core/scoped-logging-with-serilog/
        /// </summary>
        public static IHostBuilder RegisterLogger(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);

                    //https://andrewlock.net/adding-serilog-to-the-asp-net-core-generic-host/
                    loggerConfiguration
                                       //.Enrich.WithCorrelationIdHeader()
                                       //.Enrich.WithEnvironmentUserName()
                                       //.Enrich.WithMachineName()
                                       //.Enrich.WithProcessId()
                                       //.Enrich.WithProcessName()
                                       //.Enrich.WithThreadId()
                                       //.Enrich.WithMemoryUsage()
                                       //.Enrich.FromLogContext()

                                       .Enrich.WithProperty("Version", PlatformServices.Default.Application.ApplicationVersion)
                                       .Enrich.WithDynamicProperty("MemoryUsage", () => LoggerHelper.GetMemoryUsage(3))
                                       .Enrich.WithDynamicProperty("ThreadMemoryUsage", () => LoggerHelper.GetThreadMemoryUsage(3))
                                       .Enrich.WithDynamicProperty("Generations", () => LoggerHelper.GetGenerations());

                }, writeToProviders: true);
        }
    }
}