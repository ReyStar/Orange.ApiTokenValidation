using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Orange.ApiTokenValidation.API.Middleware;
using Orange.ApiTokenValidation.API.Registration;
using Orange.ApiTokenValidation.API.Services;

namespace Orange.ApiTokenValidation.API
{
    public static class DependenciesRegistrator
    {
        public static void RegisterApi(this IServiceCollection collection, HostBuilderContext hostBuilderContext)
        {
            collection.AddSingleton<IHealthCheck, HealthCheck>();

            collection.AddSingleton<Profile, AutoMapperProfile>();
            
            collection.AddSingleton<CorrelationIdMiddleware>(); //I can't see benefits if I will use AddTransient in this place
            
            collection.AddSingleton<RequestWriterMiddleware>();
        }
    }
}
