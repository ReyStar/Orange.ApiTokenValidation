using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orange.ApiTokenValidation.API.Attributes;
using Orange.ApiTokenValidation.API.Controllers.V1.DTO;
using Orange.ApiTokenValidation.Domain.Exceptions;
using Orange.ApiTokenValidation.Domain.Interfaces;

namespace Orange.ApiTokenValidation.API.Controllers.V1
{
    /// <summary>
    /// Token validation controller
    /// </summary>
    [ApiController]
    [VersionRoute("[controller]")]
    [RequireHttps]
    [NullModelValidation]
    [ValidateModel]
    [ApiVersion("1")]
    public class TokenValidationController : ControllerBase
    {
        private readonly ITokenValidationService _tokenValidationService;
        private readonly IMapper _mapper;
        private readonly ILogger<TokenValidationController> _logger;

        /// <summary>
        /// .ctor
        /// </summary>
        public TokenValidationController(ITokenValidationService tokenValidationService, IMapper mapper, ILogger<TokenValidationController> logger)
        {
            _tokenValidationService = tokenValidationService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Validate request
        /// </summary>
        [Route("audience/{audience}/validate/token")]
        [HttpPost]
        [ProducesResponseType(typeof(TokenValidationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TokenValidationResponse), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> Validate([FromRoute] string audience, [FromBody] TokenValidationRequest validationRequest)
        {
            try
            {
                var result = await _tokenValidationService.Validate(audience, validationRequest.Token);

                return Ok(_mapper.Map<TokenValidationResponse>(result));
            }
            catch (TokenValidationException ex)
            {
                var validationProblem = new ValidationProblemDetails
                {
                    Detail = ex.Message
                };

                return ValidationProblem(validationProblem);
            }
        }
    }
}
