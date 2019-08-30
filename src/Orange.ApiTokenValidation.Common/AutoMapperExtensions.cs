using System.Collections.Generic;
using Autofac;
using AutoMapper;

namespace Orange.ApiTokenValidation.Common
{
    public static class AutoMapperExtensions
    {
        public static void RegisterAutoMapper(this ContainerBuilder builder)
        {
            builder.Register(c => new MapperConfiguration(cfg =>
                {
                    //add your profiles (either resolve from container or however else you acquire them)
                    foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                    {
                        cfg.AddProfile(profile);
                    }
                })).AsSelf()
                .SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>()
                    .CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
    }
}
