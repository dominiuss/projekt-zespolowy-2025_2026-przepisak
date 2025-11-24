
namespace PrzepisakApi.src.Features.Recipes.Application.DTOs
{
    public class RecipeDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public int PreparationTime { get; set; }
        public int CookTime { get; set; }
        public int Servings { get; set; }
        public string CategoryName { get; set; }
        public string Cuisine { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
<<<<<<< HEAD
        public double AverageRating { get; set; }
        public int RatingsCount { get; set; }
=======
        public List<AddUpdateRecipeIngredientDTO> RecipeIngredients { get; set; } = new();
>>>>>>> f627f48032be820809713083c5a7e22687c44da2
    }
}
