using Dapper;
using Microsoft.EntityFrameworkCore;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Ratings.Application.DTOs;
using PrzepisakApi.src.Features.Ratings.Domain;
using System.Diagnostics.CodeAnalysis;

namespace PrzepisakApi.src.Features.Ratings.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class RatingRepository : IRatingRepository
    {
        private readonly IEfContext _efContext;
        private readonly DapperContext _dapperContext;

        public RatingRepository(IEfContext efContext, DapperContext dapperContext)
        {
            _efContext = efContext;
            _dapperContext = dapperContext;
        }

        public async Task AddAsync(Rating rating)
        {
            await _efContext.Ratings.AddAsync(rating);
        }

        public async Task<Rating?> GetUserRatingForRecipeAsync(int userId, int recipeId)
        {
            return await _efContext.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.RecipeId == recipeId);
        }

        public async Task<List<RatingDTO>> GetRatingsByRecipeIdAsync(int recipeId)
        {
            using var connection = _dapperContext.CreateConnection();
            var sql = @"
                SELECT 
                    r.id AS Id,
                    iu.""UserName"" AS UserName,
                    r.score AS Score,
                    r.comment AS Comment,
                    r.created_at AS CreatedAt
                FROM ratings r
                JOIN users u ON u.id = r.user_id
                JOIN ""AspNetUsers"" iu ON iu.""Id"" = u.identity_user_id
                WHERE r.recipe_id = @RecipeId
                ORDER BY r.created_at DESC;
            ";

            var result = await connection.QueryAsync<RatingDTO>(sql, new { RecipeId = recipeId });
            return result.ToList();
        }
    }
}