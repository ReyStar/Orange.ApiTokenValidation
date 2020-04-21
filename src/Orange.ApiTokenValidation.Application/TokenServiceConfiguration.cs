using System;

namespace Orange.ApiTokenValidation.Application
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
