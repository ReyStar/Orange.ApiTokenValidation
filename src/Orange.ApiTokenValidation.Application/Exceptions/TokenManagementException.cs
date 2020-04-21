using System;

namespace Orange.ApiTokenValidation.Application.Exceptions
{
    public class TokenManagementException : Exception
    {
        public TokenManagementException(string message) : base(message)
        {
        }

        public TokenManagementException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
