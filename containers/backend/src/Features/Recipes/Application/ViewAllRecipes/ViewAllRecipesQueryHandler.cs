using MediatR;
using PrzepisakApi.src.Features.Recipes.Domain;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;

namespace PrzepisakApi.src.Features.Recipes.Application.ViewAllRecipes
{
    public class ViewAllRecipesQueryHandler : IRequestHandler<ViewAllRecipesQuery, List<RecipeOverviewDTO>>
    {
        private readonly IRecipeRepository _recipeRepository;

        public ViewAllRecipesQueryHandler(IRecipeRepository recipeRepository) {
            _recipeRepository = recipeRepository;
        }
        public async Task<List<RecipeOverviewDTO>> Handle(ViewAllRecipesQuery request, CancellationToken cancellationToken)
        {
            var recipes = await _recipeRepository.GetAllRecipesAsync();
            return recipes;
        }
    }
}
