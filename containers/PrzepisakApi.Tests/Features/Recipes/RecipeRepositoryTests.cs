using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Recipes.Domain;
using PrzepisakApi.src.Features.Recipes.Infrastructure;

namespace PrzepisakApi.Tests.Features.Recipes
{
    public class RecipeRepositoryTests
    {
        private readonly EfContext _efContext;
        private readonly RecipeRepository _repository;

        public RecipeRepositoryTests()
        {

            var options = new DbContextOptionsBuilder<EfContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _efContext = new EfContext(options);

            _repository = new RecipeRepository(_efContext, null!);
        }

        [Fact]
        public void Add_ShouldAddRecipeToDatabase()
        {
            // Arrange
            var recipe = new Recipe
            {
                Title = "Test Recipe",
                Description = "Desc",
                Instructions = "Instr",
                PreparationTime = 10,
                CookTime = 10,
                Servings = 2,
                AuthorId = 1,
                // POPRAWKA: Uzupełniono wymagane pola
                Cuisine = "Polish",
                ImageUrl = "http://example.com/image.jpg"
            };

            // Act
            var result = _repository.Add(recipe);
            _efContext.SaveChanges(); // Symulujemy zatwierdzenie transakcji

            // Assert
            result.Should().NotBeNull();
            var addedRecipe = _efContext.Recipes.FirstOrDefault(r => r.Id == result.Id);
            addedRecipe.Should().NotBeNull();
            addedRecipe!.Title.Should().Be("Test Recipe");
        }

        [Fact]
        public void Delete_ShouldRemoveRecipe_WhenExists()
        {
            // Arrange
            var recipe = new Recipe
            {
                Id = 10,
                Title = "To Delete",
                Description = "Desc",
                Instructions = "Instr",
                PreparationTime = 10,
                CookTime = 10,
                Servings = 2,
                AuthorId = 1,
                // POPRAWKA: Uzupełniono wymagane pola
                Cuisine = "Italian",
                ImageUrl = "http://example.com/img.png"
            };

            _efContext.Recipes.Add(recipe);
            _efContext.SaveChanges();

            // Act
            _repository.Delete(10);
            _efContext.SaveChanges();

            // Assert
            var deletedRecipe = _efContext.Recipes.FirstOrDefault(r => r.Id == 10);
            deletedRecipe.Should().BeNull();
        }

        [Fact]
        public void Delete_ShouldDoNothing_WhenRecipeDoesNotExist()
        {
            // Arrange
            // Baza jest pusta

            // Act
            var action = () =>
            {
                _repository.Delete(999);
                _efContext.SaveChanges();
            };

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Update_ShouldUpdateFields_WhenRecipeExists()
        {
            // Arrange
            var originalRecipe = new Recipe
            {
                Id = 20,
                Title = "Original Title",
                Description = "Original Desc",
                Instructions = "Instr",
                PreparationTime = 10,
                CookTime = 10,
                Servings = 2,
                AuthorId = 1,
                Cuisine = "Italian",
                ImageUrl = "http://original.com/img.jpg"
            };
            _efContext.Recipes.Add(originalRecipe);
            _efContext.SaveChanges();

            // Tworzymy obiekt z nowymi danymi
            var updatedData = new Recipe
            {
                Id = 20,
                Title = "Updated Title",
                Description = "Updated Desc",
                Instructions = "Updated Instr",
                PreparationTime = 20,
                CookTime = 30,
                Servings = 4,
                Cuisine = "Polish",
                ImageUrl = "http://new.img/test.jpg",
                AuthorId = 1,
                CategoryId = 5
            };

            // Act
            var result = _repository.Update(updatedData);
            _efContext.SaveChanges();

            // Assert
            result.Should().NotBeNull();

            var recipeInDb = _efContext.Recipes.First(r => r.Id == 20);
            recipeInDb.Title.Should().Be("Updated Title");
            recipeInDb.Description.Should().Be("Updated Desc");
            recipeInDb.Cuisine.Should().Be("Polish"); // Sprawdzamy czy zaktualizowano
            recipeInDb.ImageUrl.Should().Be("http://new.img/test.jpg");
        }

        [Fact]
        public void Update_ShouldReturnNull_WhenRecipeDoesNotExist()
        {
            // Arrange
            var recipeToUpdate = new Recipe
            {
                Id = 999,
                Title = "Ghost",
                Cuisine = "None",
                ImageUrl = "None",
                Description = "None",
                Instructions = "None"
            };

            // Act
            var result = _repository.Update(recipeToUpdate);

            // Assert
            result.Should().BeNull();
        }
    }
}