using System.Diagnostics.CodeAnalysis;

namespace PrzepisakApi.src.Features.Recipes.Application.DTOs
{
    [ExcludeFromCodeCoverage]
    public class AddRecipeDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public int PreparationTime { get; set; }
        public int CookTime { get; set; }
        public int Servings { get; set; }
        public string CategoryName { get; set; }
        public string Cuisine { get; set; }
        public string ImageUrl { get; set; }
        public List<AddUpdateRecipeIngredientDTO> RecipeIngredients { get; set; } = new();
    }
}