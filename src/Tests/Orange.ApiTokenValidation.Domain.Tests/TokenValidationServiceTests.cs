using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using Orange.ApiTokenValidation.Domain.Exceptions;
using Orange.ApiTokenValidation.Domain.Interfaces;
using Orange.ApiTokenValidation.Domain.Models;
using Orange.ApiTokenValidation.Domain.Services;

namespace Orange.ApiTokenValidation.Domain.Tests
{
    public class TokenValidationServiceTests
    {
        private byte[] _privateKey;
        private readonly JwtSecurityTokenHandler _handler;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IPasswordProvider _passwordProvider;
        private string _issuer;
        private string _audience;
        private readonly TimeSpan _ttl;
        private ITokenValidationService _tokenService;
        private const int PasswordLength = 256;
        private Mock<ITokenRepository> _tokenRepository;
        private TokenServiceConfiguration _tokenServiceConfiguration;
        private TokenDescriptor _tokenDescriptor;
        private readonly Fixture _fixture;
        private SecurityToken _token;
        private TokenModel _tokenModel;
        private const int TtlSeconds = 300;
        private readonly TimeSpan _clockSkew;

        public TokenValidationServiceTests()
        {
            _handler = new JwtSecurityTokenHandler();
            _tokenGenerator = new TokenGenerator();
            _passwordProvider = new PasswordProvider();
            _ttl = new TimeSpan(0, 0, TtlSeconds);
            _clockSkew = new TimeSpan(0, 0, 30);
            _fixture = new Fixture();
        }

        [SetUp]
        public void Initialize()
        {
            _privateKey = _passwordProvider.GetNextBytePassword(PasswordLength);
            _issuer = _fixture.Create<string>();
            _audience = _fixture.Create<string>();

            _tokenServiceConfiguration = new TokenServiceConfiguration
            {
                ClockSkew = TimeSpan.Zero,
            };

            _tokenDescriptor = new TokenDescriptor
            {
                Issuer = _issuer,
                Audience = _audience,
                Ttl = TtlSeconds,
                ExpirationDate = DateTime.UtcNow.AddSeconds(30),
                PrivateKey = Convert.ToBase64String(_privateKey),
                IsActive = true
            };

            _tokenRepository = new Mock<ITokenRepository>();

            _tokenRepository.Setup(x => x.GetAsync(_issuer, _audience, CancellationToken.None))
                            .ReturnsAsync(_tokenDescriptor);

            _token = _tokenGenerator.CreateToken(_issuer, _audience, _privateKey, _ttl);
            var stringToken = _handler.WriteToken(_token);
            _tokenModel = new TokenModel(stringToken);

            _tokenService = new TokenValidationService(_tokenRepository.Object, _tokenGenerator, _tokenServiceConfiguration);
        }

        [Test]
        public async Task ValidateTokenTest_Successful()
        {
            // Arrange

            // Act
            var result = await _tokenService.ValidateAsync(_audience, _tokenModel);

            // Assert
            result.Should().NotBeNull();
            _tokenRepository.VerifyAll();
        }


        [Test]
        public void ValidateTokenTest_AudienceIsNullOrEmpty()
        {
            // Arrange

            // Act
            var result = Assert.CatchAsync<ArgumentException>(() => _tokenService.ValidateAsync(null, _tokenModel));

            // Assert
            result.Should().NotBeNull();
            result.ParamName.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public void ValidateTokenTest_TokenIsNull()
        {
            // Arrange
            
            // Act
            var result = Assert.CatchAsync<ArgumentException>(() => _tokenService.ValidateAsync(_audience, null));

            // Assert
            result.Should().NotBeNull();
            result.ParamName.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public void ValidateTokenTest_TokenIsNullOrEmpty()
        {
            // Arrange
            
            // Act
            var result = Assert.CatchAsync<ArgumentException>(() => _tokenService.ValidateAsync(_audience, new TokenModel()));

            // Assert
            result.Should().NotBeNull();
            result.ParamName.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        [AutoData]
        public void ValidateTokenTest_TokenErrorFormat(TokenModel tokenModel)
        {
            // Arrange
            
            // Act
            var result = Assert.CatchAsync<TokenValidationException>(() => _tokenService.ValidateAsync(_audience, tokenModel));

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Be(I18n.TokenMalformed);
        }

        [Test]
        public void ValidateTokenTest_EmptyIssuer()
        {
            // Arrange
            var token = _tokenGenerator.CreateToken(String.Empty, _audience, _privateKey, _ttl);
            var stringToken = _handler.WriteToken(token);

            // Act
            var result = Assert.CatchAsync<TokenValidationException>(() => _tokenService.ValidateAsync(_audience, new TokenModel(stringToken)));

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Be(I18n.NoIssuer);
        }

        [Test]
        [AutoData]
        public void ValidateTokenTest_AudienceNotContain(string audience)
        {
            // Arrange
            var token = _tokenGenerator.CreateToken(_issuer, audience, _privateKey, _ttl);
            var stringToken = _handler.WriteToken(token);

            // Act
            var result = Assert.CatchAsync<TokenValidationException>(() => _tokenService.ValidateAsync(_audience, new TokenModel(stringToken)));

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Be(I18n.TokenAudienceError);
        }

        [Test]
        public void ValidateTokenTest_TokenNotRegistered()
        {
            // Arrange
            _tokenRepository.Setup(x => x.GetAsync(_issuer, _audience, CancellationToken.None)).ReturnsAsync((TokenDescriptor) null);

            // Act
            var result = Assert.CatchAsync<TokenValidationException>(() => _tokenService.ValidateAsync(_audience, _tokenModel));

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Be(I18n.NotRegistered);
        }

        [Test]
        public void ValidateTokenTest_TokenIsDisabled()
        {
            // Arrange
            _tokenDescriptor.IsActive = false;

            // Act
            var result = Assert.CatchAsync<TokenValidationException>(() => _tokenService.ValidateAsync(_audience, _tokenModel));

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Be(I18n.TokenDisabled);
        }

        [Test]
        public void Authenticate_AuthenticationTokenException_TokenTtlLarge()
        {
            // Arrange
            _tokenDescriptor.Ttl = 0;

            // Act
            var result =
                Assert.CatchAsync<TokenValidationException>(() => _tokenService.ValidateAsync(_audience, _tokenModel));

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Be(I18n.TokenTtlLarge);
            _tokenRepository.VerifyAll();
        }

        [Test]
        public void Validate_PrivateKeyError()
        {
            // Arrange
            var token = _tokenGenerator.CreateToken(_issuer, _audience, _fixture.Create<string>(), _ttl);
            var stringToken = _handler.WriteToken(token);

            // Act
            var result = Assert.CatchAsync<TokenValidationException>(() => _tokenService.ValidateAsync(_audience, new TokenModel(stringToken)));

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Be(I18n.TokenSignatureError);
        }

        [Test]
        public void Validate_TokenExpiredError()
        {
            // Arrange
            var currentDateTime = DateTime.UtcNow;

            var notBefore = currentDateTime.AddMinutes(-3);
            var expires = notBefore.AddMinutes(1);

            var token = _tokenGenerator.CreateToken(_issuer, _audience, _privateKey, notBefore, expires);
            var stringToken = _handler.WriteToken(token);

            // Act
            var result = Assert.CatchAsync<TokenValidationException>(() => _tokenService.ValidateAsync(_audience, new TokenModel(stringToken)));

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Be(I18n.TokenExpiredError);
        }

        [Test]
        public void Validate_ClockSkewError()
        {
            // Arrange
            var currentDateTime = DateTime.UtcNow;

            var notBefore = currentDateTime.Add(_clockSkew.Negate()).Add(_ttl.Negate()).AddSeconds(-1);
            var expires = notBefore.Add(_ttl);

            var token = _tokenGenerator.CreateToken(_issuer, _audience, _privateKey, notBefore, expires);
            var stringToken = _handler.WriteToken(token);

            // Act
            var result = Assert.CatchAsync<TokenValidationException>(() => _tokenService.ValidateAsync(_audience, new TokenModel(stringToken)));

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Be(I18n.TokenExpiredError);
        }
    }
}