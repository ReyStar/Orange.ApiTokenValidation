using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orange.ApiTokenValidation.Application.Interfaces;
using Orange.ApiTokenValidation.Common;
using Orange.ApiTokenValidation.Repositories.Repositories;

namespace Orange.ApiTokenValidation.Repositories
{
    public static class DependenciesRegistrator
    {
        public static void RegisterRepositoriesDependencies(this IServiceCollection collection, HostBuilderContext context)
        {
            collection.Configure<ConnectionStrings>(context.Configuration.GetSection(nameof(ConnectionStrings)));

            collection.AddSingleton<IDataSource, DataSource>();
            collection.AddSingleton<ITokenRepository, TokenRepository>();
            
            collection.AddSingleton<Profile, AutoMapperProfile>();
            collection.AddSingleton<IFunctionalityValidator, DataBaseFunctionalityValidator>();
        }
    }
}
