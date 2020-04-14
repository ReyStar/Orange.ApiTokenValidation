using System.ComponentModel.DataAnnotations;

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
        public string Token { get; set; }
    }
}
