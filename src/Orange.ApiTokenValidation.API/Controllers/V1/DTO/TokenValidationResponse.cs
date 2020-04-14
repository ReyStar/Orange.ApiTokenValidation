namespace Orange.ApiTokenValidation.API.Controllers.V1.DTO
{
    /// <summary>
    /// Response on validation
    /// </summary>
    public class TokenValidationResponse
    {
        /// <summary>
        /// Expiration in seconds
        /// </summary>
        public double Expiration { get; set; }
    }
}
