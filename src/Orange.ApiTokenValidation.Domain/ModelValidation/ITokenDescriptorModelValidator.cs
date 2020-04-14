using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.Domain.ModelValidation
{
    interface ITokenDescriptorModelValidator
    {
        void EnsureIssuerValidation(string issuer);
        void EnsureAudienceValidation(string audience);
        void EnsureValidation(TokenDescriptor descriptor);
    }
}
