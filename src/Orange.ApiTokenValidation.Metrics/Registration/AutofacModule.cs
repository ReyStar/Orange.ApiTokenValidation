using Autofac;
using Orange.ApiTokenValidation.Domain.Interfaces;

namespace Orange.ApiTokenValidation.Metrics.Registration
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<Measurer>().As<IMeasurer>().SingleInstance();

            builder.RegisterBuildCallback(x => x.Resolve<IMeasurer>());
        }
    }
}
