using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.Configuration;

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
    }
}
