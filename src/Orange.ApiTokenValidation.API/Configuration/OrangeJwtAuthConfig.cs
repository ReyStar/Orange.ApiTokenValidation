using Microsoft.Extensions.DependencyInjection;
using Orange.ApiTokenValidation.API.Auth;

namespace Orange.ApiTokenValidation.API.Configuration
{
    public static class OrangeJwtAuthConfig
    {
        private const string DefaultScheme = "Bearer";

        public static IServiceCollection AuthenticationConfigure(this IServiceCollection services)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = DefaultScheme;
                x.DefaultChallengeScheme = DefaultScheme;
            }).AddScheme<OrangeJwtAuthenticationSchemeOptions, OrangeJwtAuthenticationHandler>(DefaultScheme, null);

            return services;
        }
    }
}
