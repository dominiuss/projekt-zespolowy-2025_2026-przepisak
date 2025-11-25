using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PrzepisakApi.src.Features.UserProfile.Api;
using PrzepisakApi.src.Features.UserProfile.Application.GetUserProfile;
using PrzepisakApi.src.Features.UserProfile.Application.UpdateUserProfile;
using PrzepisakApi.src.Features.UserProfiles.Application.DTOs;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace PrzepisakApi.Tests.Features.UserProfiles
{
    public class UserProfilesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserProfilesController _controller;

        public UserProfilesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UserProfilesController(_mediatorMock.Object);
        }

        private void SetupUser(string profileId)
        {
            var claims = new List<Claim>();
            if (profileId != null)
            {
                claims.Add(new Claim("UserProfileId", profileId));
            }

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task Get_ShouldReturnProfile_WhenAuthorized()
        {
            // Arrange
            SetupUser("100");
            var profileDto = new UserProfileDTO { Id = 100, Username = "Test" };

            _mediatorMock.Setup(m => m.Send(It.Is<GetUserProfileQuery>(q => q.UserId == 100), default))
                .ReturnsAsync(profileDto);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var val = okResult.Value.Should().BeOfType<UserProfileDTO>().Subject;
            val.Id.Should().Be(100);
        }

        [Fact]
        public async Task Get_ShouldThrow_WhenClaimMissing()
        {
            // Arrange
            SetupUser(null); // Brak claima UserProfileId

            // Act
            Func<Task> act = async () => await _controller.Get();

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Token nie zawiera UserProfileId.");
        }

        [Fact]
        public async Task Update_ShouldReturnOk()
        {
            // Arrange
            SetupUser("100");
            var request = new UpdateProfileRequestDto { Bio = "New Bio" };
            var response = new UserProfileDTO { Bio = "New Bio" };

            _mediatorMock.Setup(m => m.Send(It.Is<UpdateUserProfileCommand>(c => c.UserId == 100 && c.Bio == "New Bio"), default))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Update(request);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        }
    }
}