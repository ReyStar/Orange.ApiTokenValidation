using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orange.ApiTokenValidation.Common;
using Orange.ApiTokenValidation.Domain.Exceptions;
using Orange.ApiTokenValidation.Domain.Interfaces;
using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.API.Auth
{
    /// <summary>
    /// Custom auth header
    /// https://joonasw.net/view/creating-auth-scheme-in-aspnet-core-2
    /// </summary>
    public class OrangeJwtAuthenticationHandler: AuthenticationHandler<OrangeJwtAuthenticationSchemeOptions>
    {
        private readonly ITokenValidationService _tokenValidationService;
        private readonly string _audience;
        private const string AuthorizationHeaderKey = "Authorization";

        /// <summary>
        /// .ctor
        /// </summary>
        public OrangeJwtAuthenticationHandler(IOptionsMonitor<OrangeJwtAuthenticationSchemeOptions> options, 
                                              ILoggerFactory logger, 
                                              UrlEncoder encoder, 
                                              ISystemClock clock,
                                              ITokenValidationService tokenValidationService,
                                              IOptions<CommonConfiguration> commonConfiguration) 
            : base(options, logger, encoder, clock)
        {
            _tokenValidationService = tokenValidationService;
            _audience = commonConfiguration.Value.InstanceId;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(AuthorizationHeaderKey))
            {
                Logger.LogDebug("Request doesn't contain Authorization header");
                return AuthenticateResult.NoResult();
            }

            if (!AuthenticationHeaderValue.TryParse(Request.Headers[AuthorizationHeaderKey], out AuthenticationHeaderValue headerValue))
            {
                Logger.LogDebug($"Authorization header has invalid format {Request.Headers[AuthorizationHeaderKey]}");
                return AuthenticateResult.NoResult();
            }

            if (!Scheme.Name.Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                Logger.LogDebug($"Authorization header doesn't contain {headerValue.Scheme} scheme {headerValue}");
                return AuthenticateResult.NoResult();
            }

            try
            {
                var result = await _tokenValidationService.ValidateAsync(_audience, new TokenModel(headerValue.Parameter));

                if (result.Expiration > 0)
                {
                    var identities = new List<ClaimsIdentity>
                    {
                        new ClaimsIdentity("Orange Jwt authorization")
                    };
                    var ticket = new AuthenticationTicket(new ClaimsPrincipal(identities), Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
             
                Logger.LogDebug($"Token is expired {headerValue}");
                return AuthenticateResult.NoResult();
            }
            catch (TokenValidationException ex)
            {
                Logger.LogDebug(ex, "Authenticate exception:");

                return AuthenticateResult.NoResult();
            }
        }
    }
}
