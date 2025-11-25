using FluentAssertions;
using Mapster;
using PrzepisakApi.src.Features.Recipes.Application.AddRecipe;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using PrzepisakApi.src.Features.Recipes.Application.Mappings;
using PrzepisakApi.src.Features.Recipes.Application.UpdateRecipe;
using PrzepisakApi.src.Features.Recipes.Domain;
using System;
using Xunit;

namespace PrzepisakApi.Tests.Features.Recipes
{
    public class RecipeMappingsTests
    {
        public RecipeMappingsTests()
        {
            // To uruchamia konfigurację mapstera
            RecipeMappings.RegisterMappings();
        }

        [Fact]
        public void Should_Map_Full_Flow()
        {
            // 1. Test DTO -> Command
            var dto = new AddUpdateRecipeDTO
            {
                Title = "Test",
                Description = "Desc",
                Instructions = "Instr",
                PreparationTime = 10,
                CookTime = 20,
                Servings = 4,
                CategoryName = "Soup",
                Cuisine = "PL",
                ImageUrl = "URL"
            };
            var command = dto.Adapt<AddRecipeCommand>();

            command.Title.Should().Be("Test");
            command.Description.Should().Be("Desc");
            command.CategoryName.Should().Be("Soup");

            // 2. Test Command -> Recipe
            var recipe = command.Adapt<Recipe>();
            recipe.Title.Should().Be("Test");
            recipe.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

            // 3. Test Recipe -> DTO
            var resultDto = recipe.Adapt<AddUpdateRecipeDTO>();
            resultDto.Title.Should().Be("Test");

            // 4. Test UpdateCommand -> Recipe
            var updateCmd = new UpdateRecipeCommand { Id = 1, Title = "Updated" };
            var updatedRecipe = updateCmd.Adapt<Recipe>();
            updatedRecipe.Id.Should().Be(1);
            updatedRecipe.Title.Should().Be("Updated");
        }
    }
}