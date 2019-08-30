using Autofac;
using AutoMapper;
using Orange.ApiTokenValidation.Domain.Interfaces;
using Orange.ApiTokenValidation.Repositories.Repositories;

namespace Orange.ApiTokenValidation.Repositories.Registration
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<DataSource>().As<IDataSource>().SingleInstance();
            builder.RegisterType<TokenRepository>().As<ITokenRepository>().SingleInstance();

            builder.RegisterType<AutoMapperProfile>().As<Profile>();
        }
    }
}
