using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
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
    public class TokenValidationControllerIntegrationTests
    {
        private const string ValidationRequestPath = "/v1/TokenValidation/audience/{0}/validate/token";
        private Mock<ITokenValidationService> _tokenValidationService;
        private Mock<IMapper> _mapper;
        private Mock<ILogger<TokenValidationController>> _logger;
        private TestServer _server;
        private HttpClient _client;

        [OneTimeSetUp]
        public void Initialize()
        {
            _tokenValidationService= new Mock<ITokenValidationService>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<TokenValidationController>>();
            
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>()
                                                         .UseEnvironment("Development")
                                                         .ConfigureTestServices(collection =>
                                                         {
                                                             collection.AddSingleton<ITokenValidationService>(_tokenValidationService.Object);
                                                             collection.AddSingleton<IMapper>(_mapper.Object);
                                                             collection.AddSingleton<ILogger<TokenValidationController>>(_logger.Object);
                                                         })
            );
            _client = _server.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:53247");
        }

        [OneTimeTearDown]
        public void Shutdown()
        {
            _client?.Dispose();
            _server?.Dispose();
        }

        [Test]
        [AutoData]
        public async Task GetVersion()
        {
            // Arrange

            // Act
            var result = await _client.GetAsync("version");

            // Assert
            var data = await result.Content.ReadAsStringAsync();
            data.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        [AutoData]
        public async Task ValidateAsync_Successful(string audience,
                                                   TokenValidationResult tokenValidationResult,
                                                   TokenValidationRequest validationRequest,
                                                   TokenValidationResponse tokenValidationResponse
                                                   )
        {
            // Arrange
            _tokenValidationService
                .Setup(x => x.ValidateAsync(audience, validationRequest.Token, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tokenValidationResult);

            _mapper.Setup(x => x.Map<TokenValidationResponse>(tokenValidationResult)).Returns(tokenValidationResponse);

            // Act
            var result = await _client.PostAsync(string.Format(ValidationRequestPath, audience), new JsonContent(validationRequest));

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            var resultValue = await result.Content.ReadAsAsync<TokenValidationResponse>();
            resultValue.Should().BeEquivalentTo(tokenValidationResponse);

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
                .Setup(x => x.ValidateAsync(audience, validationRequest.Token, It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // Act
            var result = await _client.PostAsync(string.Format(ValidationRequestPath, audience), new JsonContent(validationRequest));

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            var validationProblemDetails = await result.Content.ReadAsAsync<ValidationProblemDetails>();
            validationProblemDetails.Detail.Should().Be(exception.Message);

            _tokenValidationService.VerifyAll();
        }
    }
}
