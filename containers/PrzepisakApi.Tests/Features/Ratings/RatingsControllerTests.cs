using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PrzepisakApi.src.Features.Auth.Domain;
using PrzepisakApi.src.Features.Ratings.Api;
using PrzepisakApi.src.Features.Ratings.Application.AddRating;
using PrzepisakApi.src.Features.Ratings.Application.DTOs;
using PrzepisakApi.src.Features.Ratings.Application.GetRecipeRatings;
using PrzepisakApi.src.Features.UserProfile.Domain;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace PrzepisakApi.Tests.Features.Ratings
{
    public class RatingsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly RatingsController _controller;

        public RatingsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userRepoMock = new Mock<IUserRepository>();
            _controller = new RatingsController(_mediatorMock.Object, _userRepoMock.Object);
        }

        private void SetupUser(string identityId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("IdentityId", identityId)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task AddRating_ShouldReturnOk_WhenUserExists()
        {
            // Arrange
            var dto = new AddRatingDTO { RecipeId = 1, Score = 5, Comment = "Good" };
            var identityId = "guid-123";
            var userProfile = new User { Id = 10 };

            SetupUser(identityId);
            _userRepoMock.Setup(x => x.GetUserProfileAsync(identityId)).ReturnsAsync(userProfile);

            _mediatorMock.Setup(x => x.Send(It.IsAny<AddRatingCommand>(), default))
                         .ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.AddRating(dto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            _mediatorMock.Verify(x => x.Send(It.Is<AddRatingCommand>(c => c.UserId == 10 && c.RecipeId == 1), default), Times.Once);
        }

        [Fact]
        public async Task AddRating_ShouldReturnUnauthorized_WhenProfileNotFound()
        {
            // Arrange
            SetupUser("guid-123");
            _userRepoMock.Setup(x => x.GetUserProfileAsync("guid-123")).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.AddRating(new AddRatingDTO());

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task GetRatings_ShouldReturnList()
        {
            // Arrange
            var recipeId = 1;
            var ratings = new List<RatingDTO> { new RatingDTO { Score = 5 } };
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetRecipeRatingsQuery>(), default)).ReturnsAsync(ratings);

            // Act
            var result = await _controller.GetRatings(recipeId);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var value = okResult.Value.Should().BeAssignableTo<List<RatingDTO>>().Subject;
            value.Should().HaveCount(1);
        }
    }
}