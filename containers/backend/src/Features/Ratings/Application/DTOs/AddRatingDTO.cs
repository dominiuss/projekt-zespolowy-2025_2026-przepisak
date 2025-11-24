namespace PrzepisakApi.src.Features.Ratings.Application.DTOs
{
    public class AddRatingDTO
    {
        public int RecipeId { get; set; }
        public int Score { get; set; }
        public string? Comment { get; set; }
    }
}