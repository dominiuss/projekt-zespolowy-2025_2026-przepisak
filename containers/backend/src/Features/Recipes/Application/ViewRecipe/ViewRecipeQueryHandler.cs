using MediatR;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using PrzepisakApi.src.Features.Recipes.Domain;

namespace PrzepisakApi.src.Features.Recipes.Application.ViewRecipe
{
    public class ViewRecipeQueryHandler : IRequestHandler<ViewRecipeQuery, RecipeDTO>
    {
        private readonly IRecipeRepository _recipeRepository;

        public ViewRecipeQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<RecipeDTO> Handle(ViewRecipeQuery request, CancellationToken cancellationToken)
        {
            var recipe = await _recipeRepository.GetRecipeByIdAsync(request.Id);
            return recipe;
        }
    }
}