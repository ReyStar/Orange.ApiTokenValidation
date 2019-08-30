using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoFixture;
using AutoFixture.NUnit3;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Linq;
using Npgsql;
using NUnit.Framework;
using Orange.ApiTokenValidation.Common;
using Orange.ApiTokenValidation.Domain.Interfaces;
using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.Repositories.Tests
{
    public class TokenRepositoryTests
    {
        private string _testDataBase;
        private ITokenRepository _tokenRepository;
        private readonly Fixture _fixture = new Fixture();
        private readonly string _connectionString;

        public TokenRepositoryTests()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(PlatformServices.Default.Application.ApplicationBasePath)
                                                          .AddJsonFile("appsettings.json")
                                                          .AddEnvironmentVariables()
                                                          .Build();

            _connectionString = configuration.GetConnectionString("TokenDB");
        }

        [OneTimeSetUp]
        public async Task InitializeAsync()
        {
            _testDataBase = Guid.NewGuid().ToString();
            await CreateTestDbAsync();

            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(_connectionString)
                                          {
                                              Database = _testDataBase
                                          };
            _tokenRepository = CreateRepository(connectionStringBuilder.ToString());
        }

        [OneTimeTearDown]
        public async Task ShutdownAsync()
        {
            await DropTestDbAsync();
        }

        [Test]
        public async Task CreateAndGet_Successful()
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
        public async Task Get_NotFound(string issuer, string audience)
        {
            // Arrange
            // Act
            var result = await _tokenRepository.GetAsync(issuer, audience);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task MultiCreateAndGetAll()
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
        public async Task Update_Successful()
        {
            // Arrange
            var originalTokenDescriptor = GenerateTokenDescriptor();
            await _tokenRepository.AddAsync(originalTokenDescriptor);

            var newTokenDescriptor = GenerateTokenDescriptor();

            // Act
            var result = await _tokenRepository.UpdateAsync(originalTokenDescriptor.Issuer, originalTokenDescriptor.Audience, newTokenDescriptor);

            // Assert
            result.Should().BeTrue();
            var oldInsertedResult = await _tokenRepository.GetAsync(originalTokenDescriptor.Issuer, originalTokenDescriptor.Audience);
            oldInsertedResult.Should().BeNull();

            var resultOfUpdate = await _tokenRepository.GetAsync(newTokenDescriptor.Issuer, newTokenDescriptor.Audience);
            resultOfUpdate.Should().NotBeNull();
            BeEquivalentTo(resultOfUpdate, newTokenDescriptor);
        }

        [Test]
        [AutoData]
        public async Task Update_NotFound(string issuer, string audience)
        {
            // Arrange
            var newTokenDescriptor = GenerateTokenDescriptor();

            // Act
            var result = await _tokenRepository.UpdateAsync(issuer, audience, newTokenDescriptor);

            // Assert
            result.Should().BeFalse();
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
        public async Task Delete_NotFound(string issuer, string audience)
        {
            // Arrange

            // Act
            var result = await _tokenRepository.DeleteAsync(issuer, audience);

            // Assert
            result.Should().BeFalse();
        }

        private async Task CreateTestDbAsync(CancellationToken cancellationToken = default)
        {
            var createDBScript = await SqlScriptLoader.LoadAsync("CreateTokenDB");
            var createTokenTableScript = await SqlScriptLoader.LoadAsync("CreateTokenTable");
            await using (var connection = new NpgsqlConnection(_connectionString))
            {
                var createDbCommand = new CommandDefinition(string.Format(createDBScript, _testDataBase, connection.UserName),
                                                            cancellationToken: cancellationToken);
                await connection.ExecuteAsync(createDbCommand);
                await connection.OpenAsync(cancellationToken);
                await connection.ChangeDatabaseAsync(_testDataBase, cancellationToken);

                var createTokenTableCommandDefinition = new CommandDefinition(createTokenTableScript, cancellationToken: cancellationToken);
                await connection.ExecuteAsync(createTokenTableCommandDefinition);
            }
        }

        private ITokenRepository CreateRepository(string connectionString)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(new DataSourceConfiguration
            {
                ConnectionString = connectionString
            });

            builder.RegisterModule<Registration.AutofacModule>();
            builder.RegisterAutoMapper();
            var container = builder.Build();

            return container.Resolve<ITokenRepository>();
        }

        private async Task DropTestDbAsync(CancellationToken cancellationToken = default)
        {
            var dropDBScript = await SqlScriptLoader.LoadAsync("DropTokenDB");
            await using (var connection = new NpgsqlConnection(_connectionString))
            {
                var createDbCommand = new CommandDefinition(string.Format(dropDBScript, _testDataBase), 
                                                            cancellationToken: cancellationToken);
                await connection.ExecuteAsync(createDbCommand);
            }
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