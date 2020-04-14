using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orange.ApiTokenValidation.Domain.Interfaces;
using Orange.ApiTokenValidation.Domain.ModelValidation;
using Orange.ApiTokenValidation.Domain.Services;

namespace Orange.ApiTokenValidation.Domain
{
    public static class DependenciesRegistrator
    {
        public static void RegisterDomainDependencies(this IServiceCollection collection, HostBuilderContext hostBuilderContext)
        {
            collection.AddSingleton<ITokenValidationService, TokenValidationService>();

            collection.AddSingleton<ITokenGenerator, TokenGenerator>();

            collection.AddSingleton<ITokenManagementService, TokenManagementService>();

            collection.Configure<TokenServiceConfiguration>(hostBuilderContext.Configuration.GetSection(nameof(TokenServiceConfiguration)));

            collection.AddSingleton<ITokenDescriptorModelValidator, TokenDescriptorModelValidator>();
            
            collection.AddSingleton<IPasswordProvider, PasswordProvider>();
        }
    }
}
