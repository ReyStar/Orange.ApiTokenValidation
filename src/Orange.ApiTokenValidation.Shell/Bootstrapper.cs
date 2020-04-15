using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Orange.ApiTokenValidation.API;
using Orange.ApiTokenValidation.Common;
using Orange.ApiTokenValidation.Domain;
using Orange.ApiTokenValidation.Metrics;
using Orange.ApiTokenValidation.Notification;
using Orange.ApiTokenValidation.Repositories;

namespace Orange.ApiTokenValidation.Shell
{
    static class Bootstrapper
    {
        private const string AppSettingPrefix = "appsettings";
        private const string EnvironmentVariablesPrefix = "ORANGE";

        public static async Task<IHost> CreateHostAsync(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((context, builder) =>
               {
                   var applicationPath = PlatformServices.Default.Application.ApplicationBasePath;

                   builder.SetBasePath(applicationPath)
                          .AddJsonFile($"{AppSettingPrefix}.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"{AppSettingPrefix}.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                          .AddEnvironmentVariables(prefix: EnvironmentVariablesPrefix)
                          .AddCommandLine(args);
               })
               .ConfigureWebHost(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
                   webBuilder.UseKestrel((context, options) =>
                   {
                       options.Configure(context.Configuration.GetSection("Kestrel"));
                   });
               })
               .ConfigureServices((context, services) =>
               {
                   services.RegisterCommonDependencies(context);
                   services.RegisterApi(context);
                   services.RegisterDomainDependencies(context);
                   services.RegisterMetricsDependencies(context);
                   services.RegisterNotificationDependencies(context);
                   services.RegisterRepositoriesDependencies(context);
                   
                   services.RegisterAutoMapper();

                   services.AddHostedService<HeartbeatService>();
               })
               .RegisterLogger()
               .UseSystemd();

            var host = hostBuilder.Build();

            // validate configuration
            var validators = host.Services.GetServices<IFunctionalityValidator>();
            foreach (var validator in validators)
            {
                await validator.EnsureValidationAsync();
            }

            return host;
        }
    }
}
