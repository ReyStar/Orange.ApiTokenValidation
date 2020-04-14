using System;
using Newtonsoft.Json.Linq;

namespace Orange.ApiTokenValidation.Domain.Models
{
    /// <summary>
    /// The class represents a token descriptor
    /// </summary>
    public class TokenDescriptor
    {
        /// <summary> 
        /// Target system
        /// Issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary> 
        /// External users
        /// Audience
        /// </summary>
        public string Audience { get; set; }

        /// <summary> 
        /// Security key. 
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary> 
        /// Default TTL. 
        /// </summary>
        public long Ttl { get; set; }

        /// <summary> 
        /// Expiration date (UTC time). 
        /// </summary>
        public DateTimeOffset? ExpirationDate { get; set; }

        /// <summary>  
        /// A sign whether a connection of the client to the resource is allowed. 
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Some addition info
        /// </summary>
        public JObject PayLoad { get; set; }
    }
}
