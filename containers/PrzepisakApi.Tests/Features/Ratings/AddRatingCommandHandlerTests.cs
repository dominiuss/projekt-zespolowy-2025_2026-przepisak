using MediatR;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Ratings.Application.AddRating;
using PrzepisakApi.src.Features.Ratings.Domain;

namespace PrzepisakApi.Tests.Features.Ratings
{
    public class AddRatingCommandHandlerTests
    {
        private readonly Mock<IRatingRepository> _ratingRepoMock;
        private readonly Mock<IEfContext> _efContextMock;
        private readonly AddRatingCommandHandler _handler;

        public AddRatingCommandHandlerTests()
        {
            _ratingRepoMock = new Mock<IRatingRepository>();
            _efContextMock = new Mock<IEfContext>();
            _handler = new AddRatingCommandHandler(_ratingRepoMock.Object, _efContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldAddNewRating_WhenNoneExists()
        {
            // Arrange
            var command = new AddRatingCommand { UserId = 1, RecipeId = 10, Score = 5, Comment = "Super" };
            _ratingRepoMock.Setup(x => x.GetUserRatingForRecipeAsync(1, 10)).ReturnsAsync((Rating?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _ratingRepoMock.Verify(x => x.AddAsync(It.Is<Rating>(r => r.Score == 5 && r.Comment == "Super")), Times.Once);
            _efContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().Be(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldUpdateRating_WhenExists()
        {
            // Arrange
            var command = new AddRatingCommand { UserId = 1, RecipeId = 10, Score = 1, Comment = "Bad" };
            var existingRating = new Rating { Id = 50, Score = 5, Comment = "Old" };

            _ratingRepoMock.Setup(x => x.GetUserRatingForRecipeAsync(1, 10)).ReturnsAsync(existingRating);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingRating.Score.Should().Be(1);
            existingRating.Comment.Should().Be("Bad");
            _ratingRepoMock.Verify(x => x.AddAsync(It.IsAny<Rating>()), Times.Never); // Nie powinno dodać nowego
            _efContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}