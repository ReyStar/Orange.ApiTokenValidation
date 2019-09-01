using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Orange.ApiTokenValidation.API.Configuration;
using Orange.ApiTokenValidation.API.Filters;
using Orange.ApiTokenValidation.API.Services;
using Prometheus;

namespace Orange.ApiTokenValidation.API
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services
                .AddControllers(config =>
                                    {
                                        //config.EnableEndpointRouting = true;
                                        config.Filters.Add<OperationCancelledExceptionFilter>();
                                    })
            .AddControllersAsServices()// Add controllers as services so they'll be resolved.
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddNewtonsoftJson()
            .ConfigureJsonFormat();

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Optimal;
            });

            services.ConfigureApiVersioning();

            services.AddHealthChecks().AddCheck<HealthCheck>("base_health_check");

            services.ConfigureSwagger();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              IApiVersionDescriptionProvider provider)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMetricServer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            //app.UseAuthorization();

            app.UseRouting();

            app.UseHttpMetrics();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                },
                AllowCachingResponses = false
            });

            //to proxy server
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            ////example to catch un-handle exception
            //app.UseExceptionHandler(err => {
            //        err.Run(async (contex) => {

            //        });
            //    }
            //);

            if (env.IsDevelopment())
            {
                //activate swagger ui
                app.EnableSwagger(provider);
            }
        }
    }
}
