using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Orange.ApiTokenValidation.API;
using Orange.ApiTokenValidation.Common;
using Orange.ApiTokenValidation.Domain;
using Orange.ApiTokenValidation.Repositories;

namespace Orange.ApiTokenValidation.Bootstrapper
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args);
            await builder.Build().RunAsync();
            _container?.Dispose();

            await Task.Delay(10000);
        }

        static IDisposable _container;

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(builder =>
                {
                    builder.RegisterConfiguration<DataSourceConfiguration>();
                    builder.RegisterConfiguration<TokenServiceConfiguration>();

                    builder.RegisterModule<Domain.Registration.AutofacModule>();
                    builder.RegisterModule<Repositories.Registration.AutofacModule>();
                    builder.RegisterModule<API.Registration.AutofacModule>();
                    builder.RegisterModule<Notification.Registration.AutofacModule>();
                    builder.RegisterModule<Metrics.Registration.AutofacModule>();

                    builder.RegisterAutoMapper();
                }))
                .ConfigureContainer<ContainerBuilder>((_, cb) => cb.RegisterBuildCallback(c => _container = c))
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddCommandLine(args);
                    builder.AddEnvironmentVariables();

                    var applicationPath = PlatformServices.Default.Application.ApplicationBasePath;
                    builder.SetBasePath(applicationPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                })
                .ConfigureHostConfiguration(builder =>
                {
                    //var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel()
                        //    .Configure(
                        //    (webHostBuilderContext, applicationBuilder) =>
                        //{

                        //})
                        ;
                    webBuilder.ConfigureServices((context, services) =>
                    {
                        var defaultHttpsPort = context.Configuration.GetValue<int>("Kestrel:DefaultHttpsPort");
                        services.AddHttpsRedirection(options => { options.HttpsPort = defaultHttpsPort; });
                    });
                })
                .ConfigureServices((context, collection) =>
                {
                    // collection.AddSingleton<IHostedService, HostedService>();
                })
                .RegisterLogger();

            return hostBuilder;
        }
    }
}