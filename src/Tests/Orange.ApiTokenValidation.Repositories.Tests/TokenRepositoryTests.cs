using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Orange.ApiTokenValidation.Application.Interfaces;
using Orange.ApiTokenValidation.Common;
using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.Repositories.Tests
{
    public class TokenRepositoryTests
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly Fixture _fixture = new Fixture();

        public TokenRepositoryTests()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(PlatformServices.Default.Application.ApplicationBasePath)
                                                          .AddJsonFile("appsettings.json")
                                                          .AddEnvironmentVariables()
                                                          .Build();
            var hostContext = new HostBuilderContext(new Dictionary<object, object>())
            {
                Configuration = configuration
            };

            var services = new ServiceCollection();
            services.RegisterRepositoriesDependencies(hostContext);
            services.RegisterCommonDependencies(hostContext);
            services.RegisterAutoMapper();
            var container = services.BuildServiceProvider();

            _tokenRepository = container.GetService<ITokenRepository>();
        }

        [Test]
        public async Task CreateAndGetEntity_Test()
        {
            // Arrange
            var tokenDescriptor = GenerateTokenDescriptor();
            var createResult = await _tokenRepository.AddAsync(tokenDescriptor);
            createResult.Should().BeTrue();

            // Act
            var result = await _tokenRepository.GetAsync(tokenDescriptor.Issuer, tokenDescriptor.Audience);

            // Assert
            result.Should().BeEquivalentTo(tokenDescriptor, x => x.Excluding(y => y.ExpirationDate));
            result.ExpirationDate.Should().BeCloseTo(tokenDescriptor.ExpirationDate, TimeSpan.FromMilliseconds(0.001));
        }

        [Test]
        [AutoData]
        public async Task GetNotExistEntity_Test(string issuer, string audience)
        {
            // Arrange
            // Act
            var result = await _tokenRepository.GetAsync(issuer, audience);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task MultiCreateAndGetAllEntities_Test()
        {
            // Arrange
            var tokenDescriptors = _fixture.Build<TokenDescriptor>()
                                           .Do(x => x.PayLoad = JObject.FromObject(new {Value = _fixture.Create<string>()}))
                                           .CreateMany();
            var creationTask = tokenDescriptors.Select(x => _tokenRepository.AddAsync(x));
            await Task.WhenAll(creationTask);

            // Act
            var result = await _tokenRepository.GetAllAsync();

            // Assert
            result.Count().Should().BeGreaterOrEqualTo(tokenDescriptors.Count());

            foreach (var tokenDescriptor in tokenDescriptors)
            {
                result.Should().ContainEquivalentOf(tokenDescriptor, x => x.Excluding(y => y.ExpirationDate));
            }
        }

        [Test]
        public async Task UpdateExistEntity_Test()
        {
            // Arrange
            var originalTokenDescriptor = GenerateTokenDescriptor();
            var issuer = originalTokenDescriptor.Issuer;
            var audience = originalTokenDescriptor.Audience;

            var addedResult = await _tokenRepository.AddAsync(originalTokenDescriptor);
            addedResult.Should().BeTrue();

            var newTokenDescriptor = GenerateTokenDescriptor();

            // Act
            var result = await _tokenRepository.AddOrUpdateAsync(issuer, audience, newTokenDescriptor);

            // Assert
            result.Should().BeTrue();

            var resultOfUpdate = await _tokenRepository.GetAsync(issuer, audience);
            resultOfUpdate.Should().NotBeNull();
            newTokenDescriptor.Issuer = issuer;
            newTokenDescriptor.Audience = audience;
            BeEquivalentTo(resultOfUpdate, newTokenDescriptor);
        }

        [Test]
        [AutoData]
        public async Task UpdateNotExistEntity_Test(string issuer, string audience)
        {
            // Arrange
            var newTokenDescriptor = GenerateTokenDescriptor();

            // Act
            var result = await _tokenRepository.AddOrUpdateAsync(issuer, audience, newTokenDescriptor);

            // Assert
            result.Should().BeTrue();
            var newInsertedResult = await _tokenRepository.GetAsync(newTokenDescriptor.Issuer, newTokenDescriptor.Audience);
            newInsertedResult.Should().BeNull();
        }

        [Test]
        public async Task CreateAndDelete_Successful()
        {
            // Arrange
            var tokenDescriptor = GenerateTokenDescriptor();
            await _tokenRepository.AddAsync(tokenDescriptor);
            
            // Act
            var result = await _tokenRepository.DeleteAsync(tokenDescriptor.Issuer, tokenDescriptor.Audience);

            // Assert
            result.Should().BeTrue();
            var oldInsertedResult = await _tokenRepository.GetAsync(tokenDescriptor.Issuer, tokenDescriptor.Audience);
            oldInsertedResult.Should().BeNull();
        }

        [Test]
        [AutoData]
        public async Task DeleteNotExistEntity_Test(string issuer, string audience)
        {
            // Arrange

            // Act
            var result = await _tokenRepository.DeleteAsync(issuer, audience);

            // Assert
            result.Should().BeFalse();
        }
      
        private TokenDescriptor GenerateTokenDescriptor()
        {
            return _fixture.Build<TokenDescriptor>()
                           .With(x => x.PayLoad, JObject.FromObject(new { Value = _fixture.Create<string>() }))
                           .Create();
        }

        private void BeEquivalentTo(TokenDescriptor tokenDescriptor, TokenDescriptor otherTokenDescriptor)
        {
            tokenDescriptor.Should().BeEquivalentTo(otherTokenDescriptor, x => x.Excluding(y => y.ExpirationDate));
            tokenDescriptor.ExpirationDate.Should().BeCloseTo(otherTokenDescriptor.ExpirationDate, TimeSpan.FromMilliseconds(0.001));
        }
    }
}