using Autofac;
using AutoMapper;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Orange.ApiTokenValidation.API.Middleware;
using Orange.ApiTokenValidation.API.Services;

namespace Orange.ApiTokenValidation.API.Registration
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<HealthCheck>().As<IHealthCheck>().SingleInstance();

            builder.RegisterType<AutoMapperProfile>().As<Profile>();

            builder.RegisterType<CorrelationIdMiddleware>().As<CorrelationIdMiddleware>();
            builder.RegisterType<RequestWriterMiddleware>().As<RequestWriterMiddleware>();

            builder.RegisterType<HeartbeatService>().As<IHostedService>();
        }
    }
}
