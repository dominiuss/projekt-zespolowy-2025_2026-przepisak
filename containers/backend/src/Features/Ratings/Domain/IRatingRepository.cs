using PrzepisakApi.src.Features.Ratings.Application.DTOs;

namespace PrzepisakApi.src.Features.Ratings.Domain
{
    public interface IRatingRepository
    {
        Task AddAsync(Rating rating);
        Task<List<RatingDTO>> GetRatingsByRecipeIdAsync(int recipeId);
        Task<Rating?> GetUserRatingForRecipeAsync(int userId, int recipeId);
    }
}