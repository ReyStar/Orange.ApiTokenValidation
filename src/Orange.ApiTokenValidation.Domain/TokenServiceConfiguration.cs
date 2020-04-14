using System;

namespace Orange.ApiTokenValidation.Domain
{
    internal class TokenServiceConfiguration
    {
        /// <summary>
        /// Time out of sync between servers
        /// TimeSpan.Zero; or TimeSpan.FromSeconds(120);
        /// </summary>
        public TimeSpan ClockSkew { get; set; }
    }
}
