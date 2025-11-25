using PrzepisakApi.src.Features.Ratings.Application.DTOs;
using PrzepisakApi.src.Features.Ratings.Application.GetRecipeRatings;
using PrzepisakApi.src.Features.Ratings.Domain;

namespace PrzepisakApi.Tests.Features.Ratings
{
    public class GetRecipeRatingsQueryHandlerTests
    {
        private readonly Mock<IRatingRepository> _repoMock;
        private readonly GetRecipeRatingsQueryHandler _handler;

        public GetRecipeRatingsQueryHandlerTests()
        {
            _repoMock = new Mock<IRatingRepository>();
            _handler = new GetRecipeRatingsQueryHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnRatings()
        {
            // Arrange
            var list = new List<RatingDTO> { new RatingDTO { Score = 5 } };
            _repoMock.Setup(x => x.GetRatingsByRecipeIdAsync(10)).ReturnsAsync(list);

            // Act
            var result = await _handler.Handle(new GetRecipeRatingsQuery { RecipeId = 10 }, CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            result[0].Score.Should().Be(5);
        }
    }
}