using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Recipes.Domain;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using Dapper;

namespace PrzepisakApi.src.Features.Recipes.Infrastructure
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly IEfContext _efContext;
        private readonly DapperContext _dapperContext;

        public RecipeRepository(IEfContext efContext, DapperContext dapperContext)
        {
            _efContext = efContext;
            _dapperContext = dapperContext;
        }

        public async Task<List<RecipeOverviewDTO>> GetAllRecipesAsync(List<int>? categoryIds = null)
        {
            using var connection = _dapperContext.CreateConnection();

            var sql = @"
                SELECT 
                    r.title AS Title,
                    iu.""UserName"" AS AuthorName,
                    r.description AS Description,
                    r.image_url AS ImageUrl
                FROM recipes r
                JOIN users u ON u.id = r.author_id
                JOIN ""AspNetUsers"" iu ON iu.""Id"" = u.identity_user_id
                WHERE 1=1 
            ";

            if (categoryIds != null && categoryIds.Any())
            {
                sql += " AND r.category_id = ANY(@CategoryIds)";
            }

            sql += " ORDER BY r.created_at DESC;";

            var result = await connection.QueryAsync<RecipeOverviewDTO>(sql, new { CategoryIds = categoryIds?.ToArray() });

            return result.ToList();
        }

        public async Task<RecipeDTO> GetRecipeByIdAsync(int id)
        {
            using var connection = _dapperContext.CreateConnection();
            var sql = @"
                SELECT 
                    r.title AS Title,
                    iu.""UserName"" AS AuthorName,
                    r.description AS Description,
                    r.instructions AS Instructions,
                    r.preparation_time AS PreparationTime,
                    r.cook_time AS CookTime,
                    r.servings AS Servings,
                    c.name AS CategoryName,
                    r.cuisine AS Cuisine,
                    r.image_url AS ImageUrl,
                    r.created_at AS CreatedAt,
                    r.updated_at AS UpdatedAt
                FROM recipes r
                JOIN users u ON u.id = r.author_id
                JOIN ""AspNetUsers"" iu ON iu.""Id"" = u.identity_user_id
                LEFT JOIN categories c ON c.id = r.category_id
                WHERE r.id=@Id;
            ";
            var result = await connection.QuerySingleOrDefaultAsync<RecipeDTO>(sql, new { Id = id });
            return result;
        }

        public async Task<List<RecipeOverviewDTO>> SearchRecipesByTitleAsync(string title)
        {
            using var connection = _dapperContext.CreateConnection();
            var sql = @"
                SELECT 
                    r.title AS Title,
                    iu.""UserName"" AS AuthorName,
                    r.description AS Description,
                    r.image_url AS ImageUrl
                FROM recipes r
                JOIN users u ON u.id = r.author_id
                JOIN ""AspNetUsers"" iu ON iu.""Id"" = u.identity_user_id
                WHERE r.title LIKE @Title
                ORDER BY r.created_at DESC;
            ";
            var result = await connection.QueryAsync<RecipeOverviewDTO>(sql, new { Title = $"%{title}%" });
            return result.ToList();
        }

        public async Task<List<RecipeOverviewDTO>> SearchRecipesByAuthorNameAsync(string name)
        {
            using var connection = _dapperContext.CreateConnection();
            var sql = @"
                SELECT 
                    r.title AS Title,
                    iu.""UserName"" AS AuthorName,
                    r.description AS Description,
                    r.image_url AS ImageUrl
                FROM recipes r
                JOIN users u ON u.id = r.author_id
                JOIN ""AspNetUsers"" iu ON iu.""Id"" = u.identity_user_id
                WHERE iu.""UserName"" LIKE @AuthorName
                ORDER BY r.created_at DESC;
            ";
            var result = await connection.QueryAsync<RecipeOverviewDTO>(sql, new { AuthorName = $"%{name}%" });
            return result.ToList();
        }

        public Recipe Add(Recipe recipe)
        {
            _efContext.Recipes.Add(recipe);
            return recipe;
        }

        public Recipe Update(Recipe recipe)
        {
            var existingRecipe = _efContext.Recipes.FirstOrDefault(r => r.Id == recipe.Id);
            if (existingRecipe == null) return null;

            existingRecipe.Title = recipe.Title;
            existingRecipe.Description = recipe.Description;
            existingRecipe.Instructions = recipe.Instructions;
            existingRecipe.PreparationTime = recipe.PreparationTime;
            existingRecipe.CookTime = recipe.CookTime;
            existingRecipe.Servings = recipe.Servings;
            existingRecipe.Cuisine = recipe.Cuisine;
            existingRecipe.ImageUrl = recipe.ImageUrl;
            existingRecipe.AuthorId = recipe.AuthorId;
            existingRecipe.CategoryId = recipe.CategoryId;
            existingRecipe.UpdatedAt = DateTime.UtcNow;
            return existingRecipe;
        }

        public void Delete(int id)
        {
            var recipe = _efContext.Recipes.FirstOrDefault(x => x.Id == id);
            if (recipe != null) _efContext.Recipes.Remove(recipe);
        }
    }
}