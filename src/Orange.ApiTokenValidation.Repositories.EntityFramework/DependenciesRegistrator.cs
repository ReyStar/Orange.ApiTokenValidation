using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orange.ApiTokenValidation.Application.Interfaces;
using Orange.ApiTokenValidation.Common;
using Orange.ApiTokenValidation.Repositories.EntityFramework.Repositories;

namespace Orange.ApiTokenValidation.Repositories.EntityFramework
{
    public static class DependenciesRegistrator
    {
        public static void RegisterRepositoriesDependencies(this IServiceCollection collection, HostBuilderContext context)
        {
            collection.Configure<ConnectionStrings>(context.Configuration.GetSection(nameof(ConnectionStrings)));
            //collection.AddDbContext<TokenDbContext>(options => options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection")));
            
            collection.AddSingleton<ITokenRepository, TokenRepository>();
            collection.AddSingleton<Profile, AutoMapperProfile>();

            collection.AddSingleton<ITokenDbContextFactory, TokenDbContextFactory>();
            collection.AddSingleton<IFunctionalityValidator, DataBaseFunctionalityValidator>();
        }
    }
}
