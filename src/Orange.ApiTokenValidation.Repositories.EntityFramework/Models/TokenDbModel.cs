using System;

namespace Orange.ApiTokenValidation.Repositories.EntityFramework.Models
{
    /// <summary>
    /// The class represents a token descriptor
    /// </summary>
    public class TokenDbModel
    {
        /// <summary> 
        /// Issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary> 
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
        public DateTimeOffset ExpirationDate { get; set; }

        /// <summary>  
        /// A sign whether a connection of the client to the resource is allowed. 
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Some addition info
        /// </summary>
        public string PayLoad { get; set; }

        /// <summary>
        /// System field: CreatedTime
        /// </summary>
        public DateTimeOffset CreatedTime { get; set; }

        /// <summary>
        /// System field: Creator
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// System field: UpdatedTime
        /// </summary>
        public DateTimeOffset UpdatedTime { get; set; }

        /// <summary>
        /// System field: Updater
        /// </summary>
        public string Updater { get; set; }
    }
}
