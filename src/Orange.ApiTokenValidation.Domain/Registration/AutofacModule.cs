using Autofac;
using Orange.ApiTokenValidation.Domain.Interfaces;
using Orange.ApiTokenValidation.Domain.Services;

namespace Orange.ApiTokenValidation.Domain.Registration
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<TokenValidationService>().As<ITokenValidationService>().SingleInstance();

            builder.RegisterType<TokenGenerator>().As<ITokenGenerator>();
        }
    }
}
