using System.Diagnostics.CodeAnalysis;

namespace PrzepisakApi.src.Features.Ratings.Application.DTOs
{
    [ExcludeFromCodeCoverage]
    public class AddRatingDTO
    {
        public int RecipeId { get; set; }
        public int Score { get; set; }
        public string? Comment { get; set; }
    }
}