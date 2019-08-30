using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Orange.ApiTokenValidation.API.Controllers.V1.DTO
{
    /// <summary>
    /// Token DTO
    /// </summary>
    public class TokenValidationRequest
    {
        /// <summary>
        /// security token value
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [JsonProperty(Required = Required.Always)]
        [MinLength(1)]
        public string Token { get; set; }
    }
}
