using MediatR;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using PrzepisakApi.src.Features.Recipes.Domain;

namespace PrzepisakApi.src.Features.Recipes.Application.Search.SearchByRecipeTitle
{
    public class SearchByRecipeTitleQueryHandler : IRequestHandler<SearchByRecipeTitleQuery, List<RecipeOverviewDTO>>
    {
        private readonly IRecipeRepository _recipeRepository;

        public SearchByRecipeTitleQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }
        public async Task<List<RecipeOverviewDTO>> Handle(SearchByRecipeTitleQuery request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.SearchRecipesByTitleAsync(request.Title);
        }
    }
}
