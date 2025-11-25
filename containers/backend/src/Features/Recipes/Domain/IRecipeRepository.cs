using PrzepisakApi.src.Features.Recipes.Application.DTOs;

namespace PrzepisakApi.src.Features.Recipes.Domain
{
    public interface IRecipeRepository
    {
        Task<List<RecipeOverviewDTO>> GetAllRecipesAsync(List<int>? categoryIds = null, List<int>? includedIngredientIds = null,
            List<int>? excludedIngredientIds = null);

        Task<RecipeDTO> GetRecipeByIdAsync(int id);
        Recipe Add(Recipe recipe);
        void Delete(int id);
        Recipe Update(Recipe recipe);
        Task<List<RecipeOverviewDTO>> SearchRecipesByTitleAsync(string title);
        Task<List<RecipeOverviewDTO>> SearchRecipesByAuthorNameAsync(string name);
        Task<List<RecipeIngredientDTO>> GetAllIngredientsAsync();
    }
}