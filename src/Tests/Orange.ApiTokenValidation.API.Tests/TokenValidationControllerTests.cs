using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Orange.ApiTokenValidation.API.Controllers.V1;
using Orange.ApiTokenValidation.API.Controllers.V1.DTO;
using Orange.ApiTokenValidation.Domain.Exceptions;
using Orange.ApiTokenValidation.Domain.Interfaces;
using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.API.Tests
{
    public class TokenValidationControllerTests
    {
        private TokenValidationController _controller;
        private Mock<HttpContext> _httpContext;
        private Mock<ITokenValidationService> _tokenValidationService;
        private Mock<IMapper> _mapper;
        private Mock<ILogger<TokenValidationController>> _logger;

        [OneTimeSetUp]
        public async Task InitializeAsync()
        {
            _httpContext = new Mock<HttpContext>();
            _tokenValidationService= new Mock<ITokenValidationService>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<TokenValidationController>>();

            _controller = new TokenValidationController(_tokenValidationService.Object, _mapper.Object, _logger.Object)
            {
                ControllerContext = {HttpContext = _httpContext.Object}
            };
        }

        [Test]
        [AutoData]
        public async Task ValidateAsync_Successful(string audience, 
                                                   TokenValidationRequest validationRequest, 
                                                   TokenValidationResult tokenValidationResult, 
                                                   TokenValidationResponse tokenValidationResponse)
        {
            // Arrange
            _tokenValidationService
                .Setup(x => x.ValidateAsync(audience, validationRequest.Token, CancellationToken.None))
                .ReturnsAsync(tokenValidationResult);

            _mapper.Setup(x => x.Map<TokenValidationResponse>(tokenValidationResult)).Returns(tokenValidationResponse);

           // Act
           var result = await _controller.ValidateAsync(audience, validationRequest, CancellationToken.None);

            // Assert
            var okObjectResult = result.Should().BeOfType<OkObjectResult>();
            okObjectResult.Subject.Value.Should().Be(tokenValidationResponse);

            _mapper.VerifyAll();
            _tokenValidationService.VerifyAll();
        }

        [Test]
        [AutoData]
        public async Task ValidateAsync_BadRequest(string audience, 
                                                   TokenValidationRequest validationRequest, 
                                                   TokenValidationException exception)
        {
            // Arrange
            _tokenValidationService
                .Setup(x => x.ValidateAsync(audience, validationRequest.Token, CancellationToken.None))
                .ThrowsAsync(exception);

           // Act
           var result = await _controller.ValidateAsync(audience, validationRequest, CancellationToken.None);

            // Assert
            var badRequestObjectResult = result.Should().BeOfType<BadRequestObjectResult>();
            var validationProblem = badRequestObjectResult.Subject .Value.Should().BeOfType<ValidationProblemDetails>();
            validationProblem.Subject.Detail.Should().Be(exception.Message);

            _tokenValidationService.VerifyAll();
        }
    }
}
