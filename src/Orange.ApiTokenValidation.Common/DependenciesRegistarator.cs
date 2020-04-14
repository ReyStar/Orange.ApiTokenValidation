using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Orange.ApiTokenValidation.Common
{
    public static class DependenciesRegistarator
    {
        public static void RegisterCommonDependencies(this IServiceCollection collection, HostBuilderContext hostBuilderContext)
        {
            collection.Configure<CommonConfiguration>(hostBuilderContext.Configuration.GetSection(nameof(CommonConfiguration)));
        }
    }
}
