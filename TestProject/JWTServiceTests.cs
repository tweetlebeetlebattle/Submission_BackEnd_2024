using Moq;
using NUnit.Framework;
using Backend.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TestProject
{
    public class JWTServiceTests
    {
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<IUtilityService> _mockUtilityService;
        private IJWTService _jwtService;

        [SetUp]
        public void Setup()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockUtilityService = new Mock<IUtilityService>();

            _mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("a_secure_and_long_mock_key_123456789");
            _mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("test_issuer");
            _mockConfiguration.Setup(config => config["Jwt:Audience"]).Returns("test_audience");
            _mockConfiguration.Setup(config => config["Jwt:DurationInMinutes"]).Returns("30");

            _jwtService = new JWTService(_mockConfiguration.Object, _mockUtilityService.Object);
        }

        [Test]
        public void GenerateJwtTokenByEmail_ValidEmail_ReturnsToken()
        {
            // Arrange
            var email = "test@example.com";

            // Act
            var token = _jwtService.GenerateJwtTokenByEmail(email);

            // Assert
            Assert.That(token, Is.Not.Null.And.Not.Empty);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            Assert.That(jwtToken, Is.Not.Null);
            Assert.That(jwtToken.Claims, Has.Some.Matches<Claim>(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == email));
        }


        [Test]
        public void GenerateJwtTokenByEmail_NullOrEmptyEmail_ThrowsArgumentException()
        {
            // Arrange
            string email = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _jwtService.GenerateJwtTokenByEmail(email));
            Assert.That(ex.Message, Is.EqualTo("Email cannot be null or empty. (Parameter 'email')"));

            email = "";
            ex = Assert.Throws<ArgumentException>(() => _jwtService.GenerateJwtTokenByEmail(email));
            Assert.That(ex.Message, Is.EqualTo("Email cannot be null or empty. (Parameter 'email')"));
        }


        [Test]
        public void RetrieveEmailFromToken_ValidToken_ReturnsEmail()
        {
            // Arrange
            var email = "test@example.com";
            var token = _jwtService.GenerateJwtTokenByEmail(email);

            // Act
            var result = _jwtService.RetrieveEmailFromToken(token);

            // Assert
            Assert.That(result, Is.EqualTo(email));
        }

        [Test]
        public void RetrieveEmailFromToken_InvalidToken_ReturnsNull()
        {
            // Arrange
            var token = "invalid_token";

            // Act
            var result = _jwtService.RetrieveEmailFromToken(token);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetUserIdFromJwtAsync_ValidAuthorizationHeader_ReturnsUserId()
        {
            // Arrange
            var email = "test@example.com";
            var userId = "user123";
            var token = _jwtService.GenerateJwtTokenByEmail(email);

            _mockUtilityService.Setup(service => service.GetUserIdByEmailAsync(email)).ReturnsAsync(userId);

            var authorizationHeader = $"Bearer {token}";

            // Act
            var result = await _jwtService.GetUserIdFromJwtAsync(authorizationHeader);

            // Assert
            Assert.That(result, Is.EqualTo(userId));
            _mockUtilityService.Verify(service => service.GetUserIdByEmailAsync(email), Times.Once);
        }

        [Test]
        public void GetUserIdFromJwtAsync_MissingAuthorizationHeader_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            string authorizationHeader = null;

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() => _jwtService.GetUserIdFromJwtAsync(authorizationHeader));
            Assert.That(ex.Message, Is.EqualTo("Authorization header is missing."));
        }

        [Test]
        public void GetUserIdFromJwtAsync_InvalidToken_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var authorizationHeader = "Bearer invalid_token";

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() => _jwtService.GetUserIdFromJwtAsync(authorizationHeader));
            Assert.That(ex.Message, Is.EqualTo("Invalid token."));
        }
    }
}
