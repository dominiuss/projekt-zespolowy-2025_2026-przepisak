namespace PrzepisakApi.src.Features.Recipe.Domain
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; } // Foreign key to User
        public string Description { get; set; }
        public string Instructions { get; set; }
        public int PreparationTime { get; set; } // in minutes
        public int CookTime { get; set; } // in minutes
        public int Servings { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Cuisine { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    }
}
