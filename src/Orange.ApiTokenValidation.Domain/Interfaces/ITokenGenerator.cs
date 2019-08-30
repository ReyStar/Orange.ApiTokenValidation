using System;
using Microsoft.IdentityModel.Tokens;

namespace Orange.ApiTokenValidation.Domain.Interfaces
{
    internal interface ITokenGenerator
    {
        SymmetricSecurityKey CreateKey(string key);
        SecurityToken CreateToken(string issuer, string audience, byte[] privateKey, TimeSpan ttl);
        SecurityToken CreateToken(string issuer, string audience, string key, TimeSpan ttl);
        SecurityToken CreateToken(string issuer, string audience, byte[] key, DateTime notBefore, DateTime expires);
    }
}
