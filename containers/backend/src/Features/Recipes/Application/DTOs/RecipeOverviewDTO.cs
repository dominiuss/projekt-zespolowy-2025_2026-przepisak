using System.Diagnostics.CodeAnalysis;

namespace PrzepisakApi.src.Features.Recipes.Application.DTOs
{
    [ExcludeFromCodeCoverage]
    public class RecipeOverviewDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double AverageRating { get; set; }
        public int RatingsCount { get; set; }

    }
}
