using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orange.ApiTokenValidation.Domain.Interfaces;
using Orange.ApiTokenValidation.Domain.Models;
using Orange.ApiTokenValidation.Domain.ModelValidation;
using Orange.ApiTokenValidation.Domain.NotifyMessages;

namespace Orange.ApiTokenValidation.Domain.Services
{
    class TokenManagementService: ITokenManagementService
    {
        private readonly TokenServiceConfiguration _configuration;
        private readonly ITokenRepository _tokenRepository;
        private readonly ITokenNotifier _notifier;
        private readonly IPasswordProvider _passwordProvider;
        private readonly ITokenDescriptorModelValidator _tokenDescriptorModelValidator;
        private readonly ILogger _logger;

        public TokenManagementService(IOptions<TokenServiceConfiguration> configuration, 
                                      ITokenRepository tokenRepository, 
                                      ITokenNotifier notifier,
                                      IPasswordProvider passwordProvider,
                                      ITokenDescriptorModelValidator tokenDescriptorModelValidator,
                                      ILogger<TokenManagementService> logger)
        {
            _configuration = configuration.Value;
            _tokenRepository = tokenRepository;
            _notifier = notifier;
            _passwordProvider = passwordProvider;
            _tokenDescriptorModelValidator = tokenDescriptorModelValidator;
            _logger = logger;
        }

        public Task<TokenDescriptor> GetTokenAsync(string issuer, string audience, CancellationToken cancellationToken = default)
        {
            _tokenDescriptorModelValidator.EnsureAudienceValidation(audience);
            _tokenDescriptorModelValidator.EnsureIssuerValidation(issuer);

            return _tokenRepository.GetAsync(issuer, audience, cancellationToken);
        }

        public async Task AddTokenAsync(TokenDescriptor tokenDescriptor, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(tokenDescriptor.PrivateKey))
            {
                tokenDescriptor.PrivateKey = _passwordProvider.GetNextStringPassword(ValidationConstants.MaxPasswordLength);
            }

            _tokenDescriptorModelValidator.EnsureValidation(tokenDescriptor);

            await _tokenRepository.AddAsync(tokenDescriptor, cancellationToken);

            _notifier.Notify(new TokenAddedNotifyMessage(tokenDescriptor));
        }

        public async Task AddOrUpdateTokenAsync(TokenDescriptor tokenDescriptor, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(tokenDescriptor.PrivateKey))
            {
                tokenDescriptor.PrivateKey = _passwordProvider.GetNextStringPassword(ValidationConstants.MaxPasswordLength);
            }

            _tokenDescriptorModelValidator.EnsureValidation(tokenDescriptor);

            await _tokenRepository.AddOrUpdateAsync(tokenDescriptor.Issuer, tokenDescriptor.Audience, tokenDescriptor, cancellationToken);

            _notifier.Notify(new TokenUpdatedNotifyMessage(tokenDescriptor.Issuer, tokenDescriptor.Audience, tokenDescriptor.Ttl, tokenDescriptor.ExpirationDate, tokenDescriptor.IsActive));
        }

        public async Task<bool> RemoveTokenAsync(string issuer, string audience, CancellationToken cancellationToken)
        {
            _tokenDescriptorModelValidator.EnsureAudienceValidation(audience);
            _tokenDescriptorModelValidator.EnsureIssuerValidation(issuer);

            if (await _tokenRepository.DeleteAsync(issuer, audience, cancellationToken))
            {
                _notifier.Notify(new TokenRemovedNotifyMessage(issuer, audience));

                return true;
            }

            return false;
        }
    }
}
