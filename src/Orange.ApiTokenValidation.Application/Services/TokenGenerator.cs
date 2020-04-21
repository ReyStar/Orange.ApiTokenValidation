using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Orange.ApiTokenValidation.Application.Interfaces;

namespace Orange.ApiTokenValidation.Application.Services
{
    internal class TokenGenerator : ITokenGenerator
    {
        public SymmetricSecurityKey CreateKey(string key)
        {
            return CreateKey(Base64UrlEncoder.DecodeBytes(key));
        }

        public SecurityToken CreateToken(string issuer, string audience, string key, TimeSpan ttl)
        {
            return CreateToken(issuer, audience, Encoding.UTF8.GetBytes(key), ttl);
        }

        public SecurityToken CreateToken(string issuer, string audience, byte[] key, TimeSpan ttl)
        {
            var notBefore = DateTime.UtcNow;
            var expires = notBefore.Add(ttl);

            return CreateToken(issuer, audience, key, notBefore, expires);
        }

        public SecurityToken CreateToken(string issuer, string audience, byte[] key, DateTime notBefore, DateTime expires)
        {
            var securityKey = CreateKey(key);
            var singing = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(issuer: issuer,
                audience: audience,
                claims: null,
                notBefore: notBefore,
                expires: expires,
                signingCredentials: singing);
            return token;
        }

        private SymmetricSecurityKey CreateKey(byte[] key)
        {
            return new SymmetricSecurityKey(key);
        }
    }
}
