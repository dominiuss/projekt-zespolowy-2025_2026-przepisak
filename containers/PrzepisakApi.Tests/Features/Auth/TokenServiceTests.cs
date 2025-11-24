using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PrzepisakApi.api.src.Features.Auth.Infrastructure.Options;
using PrzepisakApi.api.src.Features.Auth.Services;
using PrzepisakApi.src.Features.Auth.Domain;
using System.IdentityModel.Tokens.Jwt;

namespace PrzepisakApi.Tests.Features.Auth
{
    public class TokenServiceTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IOptions<JwtSettings>> _jwtSettingsMock;
        private readonly TokenService _service;

        public TokenServiceTests()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);

            _userRepoMock = new Mock<IUserRepository>();

            var jwtSettings = new JwtSettings
            {
                SigningKey = "super_secret_key_at_least_32_chars_long_12345",
                Issuer = "test_issuer",
                Audiences = new[] { "test_audience" }
            };
            _jwtSettingsMock = new Mock<IOptions<JwtSettings>>();
            _jwtSettingsMock.Setup(s => s.Value).Returns(jwtSettings);

            _service = new TokenService(_userManagerMock.Object, _jwtSettingsMock.Object, _userRepoMock.Object);
        }

        [Fact]
        public void CreateAccessToken_ShouldGenerateValidJwtToken()
        {
            // Arrange
            var user = new IdentityUser { Id = "guid", Email = "test@test.com", UserName = "testuser" };
            var profileId = 123;

            // Act
            var tokenString = _service.CreateAccessToken(user, profileId);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            token.Issuer.Should().Be("test_issuer");
            token.Audiences.Should().Contain("test_audience");
            token.Claims.First(c => c.Type == "IdentityId").Value.Should().Be("guid");
            token.Claims.First(c => c.Type == "UserProfileId").Value.Should().Be("123");
        }
    }
}