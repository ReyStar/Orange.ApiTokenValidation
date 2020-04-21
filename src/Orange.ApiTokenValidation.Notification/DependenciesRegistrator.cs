using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orange.ApiTokenValidation.Application.Interfaces;

namespace Orange.ApiTokenValidation.Notification
{
    public static class DependenciesRegistrator
    {
        public static void RegisterNotificationDependencies(this IServiceCollection collection, HostBuilderContext hostBuilderContext)
        {
            collection.AddSingleton<TokenNotifier>();
            collection.AddSingleton<ITokenNotifier>(x => x.GetService<TokenNotifier>());

            collection.Configure<TokenNotifierConfig>(hostBuilderContext.Configuration.GetSection(nameof(TokenNotifierConfig)));
            collection.AddHostedService<MessageFlusherService>();
        }
    }
}
