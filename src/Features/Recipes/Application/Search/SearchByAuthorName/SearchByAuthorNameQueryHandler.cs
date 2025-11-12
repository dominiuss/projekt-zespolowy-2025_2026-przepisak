using MediatR;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using PrzepisakApi.src.Features.Recipes.Application.Search.SearchByAuthorName;
using PrzepisakApi.src.Features.Recipes.Domain;

namespace PrzepisakApi.src.Features.Recipes.Application.Search.SearchByAuthorsName
{
    public class SearchByAuthorNameQueryHandler : IRequestHandler<SearchByAuthorNameQuery, List<RecipeOverviewDTO>>
    {
        private readonly IRecipeRepository _recipeRepository;
        public SearchByAuthorNameQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }
        public async Task<List<RecipeOverviewDTO>> Handle(SearchByAuthorNameQuery request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.SearchRecipesByAuthorNameAsync(request.AuthorName);
        }
    }
}
