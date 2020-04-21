using System;
using Orange.ApiTokenValidation.Application.Exceptions;
using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.Application.ModelValidation
{
    class TokenDescriptorModelValidator : ITokenDescriptorModelValidator
    {
        public void EnsureIssuerValidation(string issuer)
        {
            if (issuer.Length < ValidationConstants.MinParticipantLength
                || issuer.Length > ValidationConstants.MaxParticipantLength)
            {
                throw new TokenManagementException(String.Format(I18n.IncorrectIssuerLength, ValidationConstants.MinParticipantLength, ValidationConstants.MaxParticipantLength));
            }
        }

        public void EnsureAudienceValidation(string audience)
        {
            if (audience.Length < ValidationConstants.MinParticipantLength
              || audience.Length > ValidationConstants.MaxParticipantLength)
            {
                throw new TokenManagementException(String.Format(I18n.IncorrectAudienceLength, ValidationConstants.MinParticipantLength, ValidationConstants.MaxParticipantLength));
            }
        }

        public void EnsureValidation(TokenDescriptor tokenDescriptor)
        {
            EnsureAudienceValidation(tokenDescriptor.Audience);

            EnsureIssuerValidation(tokenDescriptor.Issuer);

            if (tokenDescriptor.PrivateKey.Length < ValidationConstants.MinPasswordLength
                || tokenDescriptor.PrivateKey.Length > ValidationConstants.MaxPasswordLength)
            {
                throw new TokenManagementException(String.Format(I18n.IncorrectPrivateKeyLength, ValidationConstants.MinPasswordLength, ValidationConstants.MaxPasswordLength));
            }

            if (tokenDescriptor.Ttl < ValidationConstants.MinTtlValue || tokenDescriptor.Ttl > ValidationConstants.MaxTtlValue)
            {
                throw new TokenManagementException(String.Format(I18n.IncorrectTtlValue, ValidationConstants.MinTtlValue, ValidationConstants.MaxTtlValue));
            }
        }
    }
}
