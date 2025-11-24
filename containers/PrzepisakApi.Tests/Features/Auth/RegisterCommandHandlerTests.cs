using Microsoft.AspNetCore.Identity;
using PrzepisakApi.api.src.Features.Auth.Application.Register;
using PrzepisakApi.src.Features.Auth.Domain;
using PrzepisakApi.src.Features.UserProfile.Domain;

namespace PrzepisakApi.Tests.Features.Auth
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new RegisterCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateUserAndProfile_WhenDataIsValid()
        {
            var command = new RegisterCommand { Username = "newUser", Password = "Password123!" };
            var createdIdentityUser = new IdentityUser { Id = "new-guid-123", UserName = "newUser" };

            _userRepositoryMock.Setup(x => x.CreateUserAsync(command.Username, command.Password))
                .ReturnsAsync(createdIdentityUser);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Message.Should().Be("Rejestracja zakończona pomyślnie.");

            _userRepositoryMock.Verify(x => x.CreateUserAsync(command.Username, command.Password), Times.Once);

            _userRepositoryMock.Verify(x => x.AddUserProfile(It.Is<User>(u => u.IdentityUserId == "new-guid-123")), Times.Once);

            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserCreationFails()
        {
            var command = new RegisterCommand { Username = "existingUser", Password = "password" };
            var expectedException = new Exception("User already exists");

            _userRepositoryMock.Setup(x => x.CreateUserAsync(command.Username, command.Password))
                .ThrowsAsync(expectedException);

            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            await action.Should().ThrowAsync<Exception>().WithMessage("User already exists");

            _userRepositoryMock.Verify(x => x.AddUserProfile(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}