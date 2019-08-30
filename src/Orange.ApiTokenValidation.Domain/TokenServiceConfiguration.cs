using System;

namespace Orange.ApiTokenValidation.Domain
{
    public class TokenServiceConfiguration
    {
        /// <summary>
        /// Time out of sync between servers
        /// TimeSpan.Zero; or TimeSpan.FromSeconds(120);
        /// </summary>
        public TimeSpan ClockSkew { get; set; }
    }
}
