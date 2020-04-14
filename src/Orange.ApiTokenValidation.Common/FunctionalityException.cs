using System;

namespace Orange.ApiTokenValidation.Common
{
    public class FunctionalityException: Exception
    {
        public FunctionalityException(string message) 
            : base(message)
        {
        }

        public FunctionalityException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
