using System.Diagnostics.CodeAnalysis;

namespace PrzepisakApi.src.Features.Recipes.Application.DTOs
{
    [ExcludeFromCodeCoverage]
    public class RecipeIngredientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
