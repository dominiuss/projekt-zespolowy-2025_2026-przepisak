using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using PrzepisakApi.src.Features.UserProfile.Application.GetUserProfile;
using PrzepisakApi.src.Features.UserProfile.Domain;
using PrzepisakApi.src.Features.UserProfiles.Application.DTOs;

namespace PrzepisakApi.Tests.Features.UserProfiles
{
    public class GetUserProfileQueryHandlerTests
    {
        private readonly Mock<IUserProfileRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetUserProfileQueryHandler _handler;

        public GetUserProfileQueryHandlerTests()
        {
            _repoMock = new Mock<IUserProfileRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetUserProfileQueryHandler(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProfile_WhenUserExists()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Bio = "Bio",
                IdentityUser = new IdentityUser { UserName = "TestUser" }
            };
            _repoMock.Setup(x => x.GetByUserIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(new GetUserProfileQuery { UserId = 1 }, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be("TestUser");
            result.Bio.Should().Be("Bio");
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenUserDoesNotExist()
        {
            _repoMock.Setup(x => x.GetByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var result = await _handler.Handle(new GetUserProfileQuery { UserId = 99 }, CancellationToken.None);

            result.Should().BeNull();
        }
    }
}