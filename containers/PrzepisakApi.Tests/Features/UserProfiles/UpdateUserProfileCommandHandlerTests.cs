using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using PrzepisakApi.src.Features.UserProfile.Application.UpdateUserProfile;
using PrzepisakApi.src.Features.UserProfile.Domain;

namespace PrzepisakApi.Tests.Features.UserProfiles
{
    public class UpdateUserProfileCommandHandlerTests
    {
        private readonly Mock<IUserProfileRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateUserProfileCommandHandler _handler;

        public UpdateUserProfileCommandHandlerTests()
        {
            _repoMock = new Mock<IUserProfileRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateUserProfileCommandHandler(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateProfileAndIdentity_WhenUserFound()
        {
            var command = new UpdateUserProfileCommand
            {
                UserId = 1,
                Bio = "New Bio",
                username = "NewName"
            };

            var identityUser = new IdentityUser { UserName = "OldName" };
            var user = new User { Id = 1, Bio = "Old Bio", IdentityUser = identityUser };

            _repoMock.Setup(x => x.GetByUserIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var result = await _handler.Handle(command, CancellationToken.None);

            user.Bio.Should().Be("New Bio");
            user.IdentityUser.UserName.Should().Be("NewName");
            user.IdentityUser.NormalizedUserName.Should().Be("NEWNAME");

            _repoMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);

            result.Bio.Should().Be("New Bio");
            result.Username.Should().Be("NewName");
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserNotFound()
        {
            _repoMock.Setup(x => x.GetByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            var action = async () => await _handler.Handle(new UpdateUserProfileCommand { UserId = 99 }, CancellationToken.None);

            await action.Should().ThrowAsync<Exception>().WithMessage("User profile not found.");
        }
    }
}