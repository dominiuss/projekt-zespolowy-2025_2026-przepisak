namespace PrzepisakApi.src.Features.Ratings.Application.DTOs
{
    public class RatingDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Score { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}