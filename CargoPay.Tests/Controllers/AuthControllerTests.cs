using CargoPay.Domain.Entities;
using CargoPay.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CargoPay.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AuthController _authController;
        private const string ValidKey = "SuperSecretKeyWithAtLeast32CharactersLength!";

        public AuthControllerTests()
        {
            _configurationMock = new Mock<IConfiguration>();

            _configurationMock.Setup(c => c["Jwt:Key"]).Returns(ValidKey);
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("issuer");
            _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("audience");

            _authController = new AuthController(_configurationMock.Object);
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsValidToken()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "admin",
                Password = "password"
            };

            // Act
            var result = _authController.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            var token = (string)okResult.Value.GetType().GetProperty("Token").GetValue(okResult.Value);

            // Token validation
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.Equal("issuer", jwtToken.Issuer);
            Assert.Contains("admin", jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "wrong",
                Password = "credentials"
            };

            // Act
            var result = _authController.Login(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}
