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
            TypeAdapterConfig<AddUpdateRecipeDTO, AddRecipeCommand>.NewConfig()
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.AuthorId, src => src.AuthorId)
                .Map(dest => dest.AuthorName, src => src.AuthorName)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Instructions, src => src.Instructions)
                .Map(dest => dest.PreparationTime, src => src.PreparationTime)
                .Map(dest => dest.CookTime, src => src.CookTime)
                .Map(dest => dest.Servings, src => src.Servings)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.CategoryName, src => src.CategoryName)
                .Map(dest => dest.Cuisine, src => src.Cuisine)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl);

            TypeAdapterConfig<AddRecipeCommand, Recipe>.NewConfig()
                .Ignore(dest => dest.Id)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.AuthorId, src => src.AuthorId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Instructions, src => src.Instructions)
                .Map(dest => dest.PreparationTime, src => src.PreparationTime)
                .Map(dest => dest.CookTime, src => src.CookTime)
                .Map(dest => dest.Servings, src => src.Servings)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.Cuisine, src => src.Cuisine)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl)
                .Map(dest => dest.CreatedAt, src => DateTime.UtcNow)
                .Map(dest => dest.UpdatedAt, src => DateTime.UtcNow);

            TypeAdapterConfig<Recipe, AddUpdateRecipeDTO>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.AuthorName, src => src.Author != null ? src.Author.IdentityUser.UserName : null)
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

            TypeAdapterConfig<UpdateRecipeCommand, Recipe>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.AuthorId, src => src.AuthorId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Instructions, src => src.Instructions)
                .Map(dest => dest.PreparationTime, src => src.PreparationTime)
                .Map(dest => dest.CookTime, src => src.CookTime)
                .Map(dest => dest.Servings, src => src.Servings)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.Cuisine, src => src.Cuisine)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl);


        }
    }
}
