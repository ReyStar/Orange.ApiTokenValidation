using System;

namespace Orange.ApiTokenValidation.Application.Exceptions
{
    public class TokenValidationException : Exception
    {
        public TokenValidationException(string message) : base(message)
        {
        }

        public TokenValidationException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
