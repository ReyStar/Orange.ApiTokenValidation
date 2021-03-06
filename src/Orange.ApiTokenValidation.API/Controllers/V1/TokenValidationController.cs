﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orange.ApiTokenValidation.API.Attributes;
using Orange.ApiTokenValidation.API.Controllers.V1.DTO;
using Orange.ApiTokenValidation.Application.Interfaces;
using Orange.ApiTokenValidation.Domain.Models;

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
    [OrangeApiVersion(ApiVersions.V1)]
    public class TokenValidationController : ControllerBase
    {
        private readonly ITokenValidationService _tokenValidationService;
        private readonly IMapper _mapper;
        private readonly ILogger<TokenValidationController> _logger;

        /// <summary>
        /// .ctor
        /// </summary>
        public TokenValidationController(ITokenValidationService tokenValidationService, 
                                         IMapper mapper,
                                         ILogger<TokenValidationController> logger)
        {
            _tokenValidationService = tokenValidationService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// ValidateAsync request
        /// </summary>
        [Route("audience/{audience}/validate/token")]
        [HttpPost]
        [ProducesResponseType(typeof(TokenValidationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TokenValidationResponse), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> ValidateAsync([FromRoute] string audience,
                                                       [FromBody] TokenValidationRequest validationRequest,
                                                       CancellationToken cancellationToken = default)
        {
            var tokenModel = _mapper.Map<TokenModel>(validationRequest);
            _logger.LogInformation("Validation request");
            
            var result = await _tokenValidationService.ValidateAsync(audience, tokenModel, cancellationToken);

            return Ok(_mapper.Map<TokenValidationResponse>(result));
        }
    }
}
