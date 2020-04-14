using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;
using Orange.ApiTokenValidation.Domain.ModelValidation;

namespace Orange.ApiTokenValidation.API.Controllers.V1.DTO
{
    public class TokenRequest
    {
        /// <summary> 
        /// Security key.
        /// Can ignore this property if want generate key is automation
        /// </summary>
        [StringLength(ValidationConstants.MaxPasswordLength, 
                      MinimumLength = ValidationConstants.MinPasswordLength)]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [System.ComponentModel.DefaultValue(null)]
        public string PrivateKey { get; set; }

        /// <summary> 
        /// Default TTL in seconds.
        /// Time of live token, when it time is over,
        /// need refresh token
        /// </summary>
        [Required]
        [System.ComponentModel.DefaultValue(ValidationConstants.MaxTtlValue)]
        [Range(ValidationConstants.MinTtlValue, ValidationConstants.MaxTtlValue)]
        public long Ttl { get; set; }

        /// <summary> 
        /// Expiration date (UTC time).
        /// Set concrete date when union of audience and issuer will be invalid
        /// </summary>
        [System.ComponentModel.DefaultValue(null)]
        public DateTimeOffset? ExpirationDate { get; set; }

        /// <summary>  
        /// A sign whether a connection of the client to the resource is allowed. 
        /// </summary>
        [Required]
        [System.ComponentModel.DefaultValue(true)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Some addition info
        /// </summary>
        [System.ComponentModel.DefaultValue(null)]
        public JObject PayLoad { get; set; }
    }
}
