namespace PrzepisakApi.src.Features.Recipes.Application.DTOs
{
    public class AddUpdateRecipeDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public int PreparationTime { get; set; }
        public int CookTime { get; set; }
        public int Servings { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Cuisine { get; set; }
        public string ImageUrl { get; set; }
        public List<AddUpdateRecipeIngredientDTO> RecipeIngredients { get; set; } = new();
    }
}
