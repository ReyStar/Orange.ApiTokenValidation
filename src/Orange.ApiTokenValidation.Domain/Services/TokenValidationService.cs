using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.IdentityModel.Tokens;
using Orange.ApiTokenValidation.Domain.Exceptions;
using Orange.ApiTokenValidation.Domain.Interfaces;
using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.Domain.Services
{
    class TokenValidationService : ITokenValidationService
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly TokenServiceConfiguration _configuration;
        private readonly ITokenRepository _tokenRepository;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public TokenValidationService(ITokenRepository tokenRepository,
                                      ITokenGenerator tokenGenerator,
                                      TokenServiceConfiguration configuration)
        {
            _tokenRepository = tokenRepository;
            _tokenGenerator = tokenGenerator;
            _configuration = configuration;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task<AuthenticationResult> Validate(string audience, string token)
        {
            EnsureArg.IsNotNullOrWhiteSpace(audience, nameof(audience));
            EnsureArg.IsNotNullOrWhiteSpace(token, nameof(token));

            if (!_tokenHandler.CanReadToken(token))
            {
                throw new TokenValidationException(I18n.TokenMalformed);
            }

            var securityToken = _tokenHandler.ReadJwtToken(token);

            var issuer = securityToken.Issuer;
            if (string.IsNullOrWhiteSpace(issuer))
            {
                throw new TokenValidationException(I18n.NoIssuer);
            }

            if (!securityToken.Audiences.Contains(audience))
            {
                throw new TokenValidationException(I18n.TokenAudienceError);
            }

            var tokenDescriptor = await _tokenRepository.GetAsync(issuer, audience) 
                                  ?? throw new TokenValidationException(I18n.NotRegistered);

            if (!tokenDescriptor.IsActive)
            {
                throw new TokenValidationException(I18n.TokenDisabled);
            }

            ValidateJwt(token, issuer, tokenDescriptor.Audience, tokenDescriptor.PrivateKey);

            if ((securityToken.ValidTo - securityToken.ValidFrom).TotalSeconds > tokenDescriptor.Ttl)
            {
                throw new TokenValidationException(I18n.TokenTtlLarge);
            }

            return new AuthenticationResult
            {
                Expiration = (long)(securityToken.ValidTo - securityToken.ValidFrom).TotalSeconds
            };
        }

        private void ValidateJwt(string token, string issuer, string audience, string key)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                RequireAudience = true,
                ClockSkew = _configuration.ClockSkew,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _tokenGenerator.CreateKey(key)
            };

            try
            {
                _tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch (SecurityTokenInvalidIssuerException ex)
            {
                throw new TokenValidationException(I18n.TokenIssuerError, ex);
            }
            catch (SecurityTokenInvalidAudienceException ex)
            {
                throw new TokenValidationException(I18n.TokenAudienceError, ex);
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
                throw new TokenValidationException(I18n.TokenSignatureError, ex);
            }
            catch (SecurityTokenExpiredException ex)
            {
                throw new TokenValidationException(I18n.TokenExpiredError, ex);
            }
            catch (SecurityTokenException ex)
            {
                throw new TokenValidationException(I18n.TokenValidationError, ex);
            }
        }
    }
}