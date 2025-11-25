using Mapster;
using PrzepisakApi.src.Features.Recipes.Application.AddRecipe;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using PrzepisakApi.src.Features.Recipes.Application.UpdateRecipe;
using PrzepisakApi.src.Features.Recipes.Domain;

namespace PrzepisakApi.src.Features.Recipes.Application.Mappings
{
    public static class RecipeMappings
    {
        public static void RegisterMappings()
        {
            // DTO → AddRecipeCommand
            TypeAdapterConfig<AddUpdateRecipeDTO, AddRecipeCommand>.NewConfig()
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Instructions, src => src.Instructions)
                .Map(dest => dest.PreparationTime, src => src.PreparationTime)
                .Map(dest => dest.CookTime, src => src.CookTime)
                .Map(dest => dest.Servings, src => src.Servings)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.CategoryName, src => src.CategoryName)
                .Map(dest => dest.Cuisine, src => src.Cuisine)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl);

            // AddRecipeCommand → Recipe
            TypeAdapterConfig<AddRecipeCommand, Recipe>.NewConfig()
                .Ignore(dest => dest.Id)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Instructions, src => src.Instructions)
                .Map(dest => dest.PreparationTime, src => src.PreparationTime)
                .Map(dest => dest.CookTime, src => src.CookTime)
                .Map(dest => dest.Servings, src => src.Servings)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.Cuisine, src => src.Cuisine)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl)
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
                .Map(dest => dest.UpdatedAt, _ => DateTime.UtcNow);

            // Recipe → DTO
            TypeAdapterConfig<Recipe, AddUpdateRecipeDTO>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.Name : null)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Instructions, src => src.Instructions)
                .Map(dest => dest.PreparationTime, src => src.PreparationTime)
                .Map(dest => dest.CookTime, src => src.CookTime)
                .Map(dest => dest.Servings, src => src.Servings)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.Cuisine, src => src.Cuisine)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl)
                .Map(dest => dest.AuthorId, src => src.AuthorId);

            // UpdateRecipeCommand → Recipe
            TypeAdapterConfig<UpdateRecipeCommand, Recipe>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Instructions, src => src.Instructions)
                .Map(dest => dest.PreparationTime, src => src.PreparationTime)
                .Map(dest => dest.CookTime, src => src.CookTime)
                .Map(dest => dest.Servings, src => src.Servings)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.Cuisine, src => src.Cuisine)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl);

            // AddRecipeDTO → AddRecipeCommand (wejściowy)
            TypeAdapterConfig<AddRecipeDTO, AddRecipeCommand>.NewConfig()
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Instructions, src => src.Instructions)
                .Map(dest => dest.PreparationTime, src => src.PreparationTime)
                .Map(dest => dest.CookTime, src => src.CookTime)
                .Map(dest => dest.Servings, src => src.Servings)
                .Map(dest => dest.CategoryName, src => src.CategoryName)
                .Map(dest => dest.Cuisine, src => src.Cuisine)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl)
                .Map(dest => dest.RecipeIngredients, src => src.RecipeIngredients);

            
        }
    }
}
