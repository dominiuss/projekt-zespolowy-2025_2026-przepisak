using FluentAssertions;
using Moq;
using PrzepisakApi.src.Features.Recipes.Application;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using PrzepisakApi.src.Features.Recipes.Application.Search.SearchByAuthorName;
using PrzepisakApi.src.Features.Recipes.Application.Search.SearchByAuthorsName;
using PrzepisakApi.src.Features.Recipes.Application.Search.SearchByRecipeTitle;
using PrzepisakApi.src.Features.Recipes.Application.ViewAllRecipes;
using PrzepisakApi.src.Features.Recipes.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PrzepisakApi.Tests.Features.Recipes
{
    public class RecipeHandlersTests
    {
        private readonly Mock<IRecipeRepository> _repoMock;

        public RecipeHandlersTests()
        {
            _repoMock = new Mock<IRecipeRepository>();
        }

        [Fact]
        public async Task Handlers_Should_Call_Repository_Methods()
        {
            // 1. Test SearchByTitle
            var titleHandler = new SearchByRecipeTitleQueryHandler(_repoMock.Object);
            var titleQuery = new SearchByRecipeTitleQuery { Title = "Soup" };

            await titleHandler.Handle(titleQuery, CancellationToken.None);
            _repoMock.Verify(x => x.SearchRecipesByTitleAsync("Soup"), Times.Once);

            // 2. Test SearchByAuthor
            var authorHandler = new SearchByAuthorNameQueryHandler(_repoMock.Object);
            var authorQuery = new SearchByAuthorNameQuery { AuthorName = "John" };

            await authorHandler.Handle(authorQuery, CancellationToken.None);
            _repoMock.Verify(x => x.SearchRecipesByAuthorNameAsync("John"), Times.Once);

            // 3. Test ViewAll
            var viewHandler = new ViewAllRecipesQueryHandler(_repoMock.Object);
            await viewHandler.Handle(new ViewAllRecipesQuery(), CancellationToken.None);
            _repoMock.Verify(x => x.GetAllRecipesAsync(null, null, null), Times.Once);

            // 4. Test Ingredients
            var ingHandler = new GetAllIngredients.Handler(_repoMock.Object);
            await ingHandler.Handle(new GetAllIngredients.Query(), CancellationToken.None);
            _repoMock.Verify(x => x.GetAllIngredientsAsync(), Times.Once);
        }
    }
}