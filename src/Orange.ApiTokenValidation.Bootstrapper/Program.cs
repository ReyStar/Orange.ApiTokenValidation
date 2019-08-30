using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
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
        public static Task Main(string[] args)
        {
            return CreateHostBuilder(args).Build()
                                          .RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var applicationPath = PlatformServices.Default.Application.ApplicationBasePath;
            
            //  var isDevelopment = environment == EnvironmentName.Development;
            //Environment
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(builder =>
                {
                    builder.RegisterConfiguration<DataSourceConfiguration>();
                    builder.RegisterConfiguration<TokenServiceConfiguration>();

                    builder.RegisterModule<Domain.Registration.AutofacModule>();
                    builder.RegisterModule<Repositories.Registration.AutofacModule>();
                    builder.RegisterModule<API.Registration.AutofacModule>();

                    builder.RegisterAutoMapper();


                }))
                .ConfigureAppConfiguration((context, builder) =>
                {
                    //builder.Add()
                })
                .ConfigureHostConfiguration(builder =>
                {
                    builder.AddCommandLine(args);
                    builder.AddEnvironmentVariables();
                    builder.SetBasePath(applicationPath)
                           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                           .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel();
                    
                    webBuilder.ConfigureServices(services =>
                    {
                    });
                });

            return hostBuilder;
        }
    }
}
