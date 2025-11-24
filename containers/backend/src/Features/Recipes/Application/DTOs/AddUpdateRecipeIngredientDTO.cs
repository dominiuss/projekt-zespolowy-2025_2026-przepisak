namespace PrzepisakApi.src.Features.Recipes.Application.DTOs
{
    public class AddUpdateRecipeIngredientDTO
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
    }
}
