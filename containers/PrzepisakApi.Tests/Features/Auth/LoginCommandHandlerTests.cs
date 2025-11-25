using Microsoft.AspNetCore.Identity;
using PrzepisakApi.api.src.Features.Auth.Application.Login;
using PrzepisakApi.api.src.Features.Auth.Services;
using PrzepisakApi.src.Features.Auth.Domain;
using PrzepisakApi.src.Features.UserProfile.Domain;

namespace PrzepisakApi.Tests.Features.Auth
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTests()
        {
            _tokenServiceMock = new Mock<ITokenService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new LoginCommandHandler(_tokenServiceMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var command = new LoginCommand { Username = "validUser", Password = "validPassword" };
            var identityUser = new IdentityUser { Id = "guid", UserName = "validUser" };
            var userProfile = new User { Id = 1, IdentityUserId = "guid" };
            var tokenResponse = new TokenResponseDto { AccessToken = "abc", RefreshToken = "xyz" };

            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(command.Username))
                .ReturnsAsync(identityUser);
            _userRepositoryMock.Setup(x => x.CheckPasswordAsync(identityUser, command.Password))
                .ReturnsAsync(true);
            _userRepositoryMock.Setup(x => x.GetUserProfileAsync(identityUser.Id))
                .ReturnsAsync(userProfile);
            _tokenServiceMock.Setup(x => x.GenerateTokensAsync(identityUser, userProfile.Id))
                .ReturnsAsync(tokenResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Token.Should().Be("abc");
            result.RefreshToken.Should().Be("xyz");
            result.ErrorMessage.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenUserNotFound()
        {
            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser?)null);

            var result = await _handler.Handle(new LoginCommand { Username = "bad", Password = "bad" }, CancellationToken.None);

            result.ErrorMessage.Should().Be("Nieprawidłowa nazwa użytkownika lub/i hasło");
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenPasswordInvalid()
        {
            var user = new IdentityUser();
            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userRepositoryMock.Setup(x => x.CheckPasswordAsync(user, It.IsAny<string>())).ReturnsAsync(false);

            var result = await _handler.Handle(new LoginCommand { Username = "u", Password = "p" }, CancellationToken.None);

            result.ErrorMessage.Should().Be("Nieprawidłowa nazwa użytkownika lub/i hasło");
        }
        [Fact]
        public async Task Handle_ShouldReturnNull_WhenUserProfileIsMissing()
        {
            var command = new LoginCommand { Username = "userWithoutProfile", Password = "password123" };
            var identityUser = new IdentityUser { Id = "identity-2", UserName = "userWithoutProfile" };

            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(command.Username))
                .ReturnsAsync(identityUser);
            _userRepositoryMock.Setup(x => x.CheckPasswordAsync(identityUser, command.Password))
                .ReturnsAsync(true);

            _userRepositoryMock.Setup(x => x.GetUserProfileAsync(identityUser.Id))
                .ReturnsAsync((User?)null);

            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().BeNull();
        }
    }
}