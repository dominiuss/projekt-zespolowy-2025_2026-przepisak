using System.Diagnostics.CodeAnalysis;

namespace PrzepisakApi.src.Features.Recipes.Application.DTOs
{
    [ExcludeFromCodeCoverage]
    public class AddUpdateRecipeIngredientDTO
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
    }
}
