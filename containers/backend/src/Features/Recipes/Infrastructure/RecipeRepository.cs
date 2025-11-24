using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Recipes.Domain;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using Dapper;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<RecipeOverviewDTO>> GetAllRecipesAsync(
            List<int>? categoryIds = null,
            List<int>? includedIngredientIds = null,
            List<int>? excludedIngredientIds = null)
        {
            using var connection = _dapperContext.CreateConnection();

            var sql = @"
                SELECT 
                    r.id as Id,
                    r.title AS Title,
                    iu.""UserName"" AS AuthorName,
                    r.description AS Description,
                    r.image_url AS ImageUrl,
                    (SELECT COALESCE(AVG(rt.score), 0) FROM ratings rt WHERE rt.recipe_id = r.id) AS AverageRating,
                    (SELECT COUNT(rt.id) FROM ratings rt WHERE rt.recipe_id = r.id) AS RatingsCount
                FROM recipes r
                JOIN users u ON u.id = r.author_id
                JOIN ""AspNetUsers"" iu ON iu.""Id"" = u.identity_user_id
                WHERE 1=1 
            ";

            if (categoryIds != null && categoryIds.Any())
            {
                sql += " AND r.category_id = ANY(@CategoryIds)";
            }

            if (includedIngredientIds != null && includedIngredientIds.Any())
            {
                sql += @" AND r.id IN (
                            SELECT ri.recipe_id 
                            FROM recipe_ingredients ri 
                            WHERE ri.ingredient_id = ANY(@IncludedIngredientIds)
                            GROUP BY ri.recipe_id
                            HAVING COUNT(DISTINCT ri.ingredient_id) = @IncludedCount
                         )";
            }

            if (excludedIngredientIds != null && excludedIngredientIds.Any())
            {
                sql += @" AND NOT EXISTS (
                            SELECT 1 
                            FROM recipe_ingredients ri 
                            WHERE ri.recipe_id = r.id 
                            AND ri.ingredient_id = ANY(@ExcludedIngredientIds)
                         )";
            }

            sql += " ORDER BY r.created_at DESC;";

            var result = await connection.QueryAsync<RecipeOverviewDTO>(sql, new
            {
                CategoryIds = categoryIds?.ToArray(),
                IncludedIngredientIds = includedIngredientIds?.ToArray(),
                IncludedCount = includedIngredientIds?.Count ?? 0,
                ExcludedIngredientIds = excludedIngredientIds?.ToArray()
            });

            return result.ToList();
        }

        public async Task<List<RecipeIngredientDTO>> GetAllIngredientsAsync()
        {
            using var connection = _dapperContext.CreateConnection();

            var sql = @"
                SELECT 
                    id as Id, 
                    name as Name 
                FROM ingredients 
                ORDER BY name ASC";

            var result = await connection.QueryAsync<RecipeIngredientDTO>(sql);
            return result.ToList();
        }

        public async Task<RecipeDTO> GetRecipeByIdAsync(int id)
        {
            using var connection = _dapperContext.CreateConnection();

            // KROK 1: Pobierz główne dane przepisu
            var sqlRecipe = @"
                SELECT
                    r.id as Id,
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
                    r.updated_at AS UpdatedAt,
                (SELECT COALESCE(AVG(rt.score), 0) FROM ratings rt WHERE rt.recipe_id = r.id) AS AverageRating,
                (SELECT COUNT(rt.id) FROM ratings rt WHERE rt.recipe_id = r.id) AS RatingsCount
                FROM recipes r
                JOIN users u ON u.id = r.author_id
                JOIN ""AspNetUsers"" iu ON iu.""Id"" = u.identity_user_id
                LEFT JOIN categories c ON c.id = r.category_id
                WHERE r.id=@Id;
            ";

            var recipe = await connection.QuerySingleOrDefaultAsync<RecipeDTO>(sqlRecipe, new { Id = id });

            if (recipe == null) return null;

            var sqlIngredients = @"
                SELECT 
                    i.id AS IngredientId,
                    i.name AS Name,
                    ri.quantity AS Quantity
                FROM recipe_ingredients ri
                JOIN ingredients i ON i.id = ri.ingredient_id
                WHERE ri.recipe_id = @Id
            ";

            var ingredients = await connection.QueryAsync<AddUpdateRecipeIngredientDTO>(sqlIngredients, new { Id = id });

            recipe.RecipeIngredients = ingredients.ToList();

            return recipe;
        }

        public async Task<List<RecipeOverviewDTO>> SearchRecipesByTitleAsync(string title)
        {
            using var connection = _dapperContext.CreateConnection();
            var sql = @"
                SELECT 
                    r.id as Id,
                    r.title AS Title,
                    iu.""UserName"" AS AuthorName,
                    r.description AS Description,
                    r.image_url AS ImageUrl,
                    (SELECT COALESCE(AVG(rt.score), 0) FROM ratings rt WHERE rt.recipe_id = r.id) AS AverageRating,
                    (SELECT COUNT(rt.id) FROM ratings rt WHERE rt.recipe_id = r.id) AS RatingsCount
                FROM recipes r
                JOIN users u ON u.id = r.author_id
                JOIN ""AspNetUsers"" iu ON iu.""Id"" = u.identity_user_id
                WHERE r.title ILIKE @Title 
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
                    r.id as Id,
                    r.title AS Title,
                    iu.""UserName"" AS AuthorName,
                    r.description AS Description,
                    r.image_url AS ImageUrl,
                    (SELECT COALESCE(AVG(rt.score), 0) FROM ratings rt WHERE rt.recipe_id = r.id) AS AverageRating,
                    (SELECT COUNT(rt.id) FROM ratings rt WHERE rt.recipe_id = r.id) AS RatingsCount
                FROM recipes r
                JOIN users u ON u.id = r.author_id
                JOIN ""AspNetUsers"" iu ON iu.""Id"" = u.identity_user_id
                WHERE iu.""UserName"" ILIKE @AuthorName
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