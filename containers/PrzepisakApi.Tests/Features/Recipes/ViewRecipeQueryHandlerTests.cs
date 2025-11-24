using PrzepisakApi.src.Features.Recipes.Application.ViewRecipe;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using PrzepisakApi.src.Features.Recipes.Domain;

namespace PrzepisakApi.Tests.Features.Recipes
{
    public class ViewRecipeQueryHandlerTests
    {
        private readonly Mock<IRecipeRepository> _repoMock;
        private readonly ViewRecipeQueryHandler _handler;

        public ViewRecipeQueryHandlerTests()
        {
            _repoMock = new Mock<IRecipeRepository>();
            _handler = new ViewRecipeQueryHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnRecipe_WhenExists()
        {
            // Arrange
            var query = new ViewRecipeQuery { Id = 123 };
            var dto = new RecipeDTO { Id = 123, Title = "Test Recipe" };

            _repoMock.Setup(x => x.GetRecipeByIdAsync(123)).ReturnsAsync(dto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(123);
            result.Title.Should().Be("Test Recipe");
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenNotExists()
        {
            _repoMock.Setup(x => x.GetRecipeByIdAsync(It.IsAny<int>())).ReturnsAsync((RecipeDTO?)null);

            var result = await _handler.Handle(new ViewRecipeQuery { Id = 0 }, CancellationToken.None);

            result.Should().BeNull();
        }
    }
}