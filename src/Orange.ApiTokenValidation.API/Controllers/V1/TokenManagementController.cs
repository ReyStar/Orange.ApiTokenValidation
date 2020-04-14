using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orange.ApiTokenValidation.API.Attributes;
using Orange.ApiTokenValidation.API.Controllers.V1.DTO;
using Orange.ApiTokenValidation.Domain.Interfaces;
using Orange.ApiTokenValidation.Domain.Models;
using Orange.ApiTokenValidation.Domain.ModelValidation;

namespace Orange.ApiTokenValidation.API.Controllers.V1
{
    /// <summary>
    /// Token management controller
    /// </summary>
    [ApiController]
    [VersionRoute("[controller]")]
    [RequireHttps]
    [NullModelValidation]
    [ValidateModel]
    [OrangeApiVersion(ApiVersions.V1)]
    //[Authorize]
    public class TokenManagementController : ControllerBase
    {
        private readonly ITokenManagementService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        /// <summary>
        /// .ctor
        /// </summary>
        public TokenManagementController(ITokenManagementService tokenService, 
                                         IMapper mapper,
                                         ILogger<TokenManagementController> logger)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// ValidateAsync request
        /// </summary>
        [Route("audience/{audience}/issuer/{issuer}/token")]
        [HttpGet]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> GetTokenAsync([FromRoute, StringLength(maximumLength: ValidationConstants.MaxParticipantLength, 
                                                                                MinimumLength = ValidationConstants.MinParticipantLength)] string audience,
                                                       [FromRoute, StringLength(maximumLength: ValidationConstants.MaxParticipantLength, 
                                                                                MinimumLength = ValidationConstants.MinParticipantLength)] string issuer, 
                                                       CancellationToken cancellationToken = default)
        {
            var result = await _tokenService.GetTokenAsync(audience, issuer, cancellationToken);
            if (result != null)
            {
                return Ok(_mapper.Map<TokenResponse>(result));
            }

            return NotFound();
        }

        /// <summary>
        /// Add api token
        /// </summary>
        [Route("audience/{audience}/issuer/{issuer}/token")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> AddTokenAsync([FromRoute, StringLength(maximumLength: ValidationConstants.MaxParticipantLength, 
                                                                                MinimumLength = ValidationConstants.MinParticipantLength)] string audience,
                                                       [FromRoute, StringLength(maximumLength: ValidationConstants.MaxParticipantLength, 
                                                                                MinimumLength = ValidationConstants.MinParticipantLength)]string issuer,
                                                       [FromBody] TokenRequest tokenRequest,
                                                       CancellationToken cancellationToken = default)
        {
            var tokenDescriptor = _mapper.Map<TokenDescriptor>(tokenRequest);

            tokenDescriptor.Issuer = issuer;
            tokenDescriptor.Audience = audience;

            await _tokenService.AddTokenAsync(tokenDescriptor, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Add or update api token
        /// </summary>
        [Route("audience/{audience}/issuer/{issuer}/token")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> AddOrUpdateTokenAsync([FromRoute, StringLength(maximumLength: ValidationConstants.MaxParticipantLength, 
                                                                                        MinimumLength = ValidationConstants.MinParticipantLength)] string audience,
                                                               [FromRoute, StringLength(maximumLength: ValidationConstants.MaxParticipantLength, 
                                                                                        MinimumLength = ValidationConstants.MinParticipantLength)] string issuer,
                                                               [FromBody] TokenRequest tokenRequest,
                                                               CancellationToken cancellationToken = default)
        {
            var tokenDescriptor = _mapper.Map<TokenDescriptor>(tokenRequest);

            tokenDescriptor.Issuer = issuer;
            tokenDescriptor.Audience = audience;

            await _tokenService.AddOrUpdateTokenAsync(tokenDescriptor, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Remove exist api token
        /// </summary>
        [Route("audience/{audience}/issuer/{issuer}/token")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> RemoveTokenAsync([FromRoute, StringLength(maximumLength: ValidationConstants.MaxParticipantLength, 
                                                                                   MinimumLength = ValidationConstants.MinParticipantLength)] string audience,
                                                          [FromRoute, StringLength(maximumLength: ValidationConstants.MaxParticipantLength, 
                                                                                   MinimumLength = ValidationConstants.MinParticipantLength)] string issuer,
                                                          CancellationToken cancellationToken = default)
        {
            var result = await _tokenService.RemoveTokenAsync(issuer, audience, cancellationToken);
            if (result)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
