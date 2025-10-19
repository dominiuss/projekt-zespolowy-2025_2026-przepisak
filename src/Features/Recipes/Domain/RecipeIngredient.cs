namespace PrzepisakApi.src.Features.Recipe.Domain
{
    public class RecipeIngredient
    {
        public int Id { get; set; }
        public int RecipeId { get; set; } // Foreign key to Recipe
        public int IngredientId { get; set; } // Foreign key to Ingredient
        public string Quantity { get; set; }
        public Recipe Recipe { get; set; }
        public Ingredient Ingredient { get; set; }

    }
}
