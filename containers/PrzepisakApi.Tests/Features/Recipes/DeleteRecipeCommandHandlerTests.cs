using MediatR;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Recipes.Application.DeleteRecipe;
using PrzepisakApi.src.Features.Recipes.Domain;

namespace PrzepisakApi.Tests.Features.Recipes
{
    public class DeleteRecipeCommandHandlerTests
    {
        private readonly Mock<IRecipeRepository> _repoMock;
        private readonly Mock<IEfContext> _efContextMock;
        private readonly DeleteRecipeCommandHandler _handler;

        public DeleteRecipeCommandHandlerTests()
        {
            _repoMock = new Mock<IRecipeRepository>();
            _efContextMock = new Mock<IEfContext>();
            _handler = new DeleteRecipeCommandHandler(_repoMock.Object, _efContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteRecipeAndSaveChanges()
        {
            // Arrange
            var command = new DeleteRecipeCommand(123);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repoMock.Verify(x => x.Delete(123), Times.Once);
            _efContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}