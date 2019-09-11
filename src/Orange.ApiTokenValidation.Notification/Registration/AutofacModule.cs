using Autofac;
using Orange.ApiTokenValidation.Domain.Interfaces;

namespace Orange.ApiTokenValidation.Notification.Registration
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<TokenNotifier>().As<ITokenNotifier>().SingleInstance();
        }
    }
}
