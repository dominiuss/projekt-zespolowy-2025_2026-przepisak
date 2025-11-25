using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Recipes.Domain;
using PrzepisakApi.src.Features.Recipes.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

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
            var recipe = new Recipe
            {
                Title = "New Recipe",
                Description = "Desc",
                Instructions = "Instr",
                Cuisine = "PL",
                ImageUrl = "Url",
                AuthorId = 1,
                PreparationTime = 10,
                CookTime = 10,
                Servings = 2
            };

            _repository.Add(recipe);
            _efContext.SaveChanges();

            _efContext.Recipes.Should().ContainSingle(r => r.Title == "New Recipe");
        }

        [Fact]
        public void Delete_ShouldRemoveRecipe_WhenExists()
        {
            var recipe = new Recipe { Id = 10, Title = "Del", Description = "D", Instructions = "I", Cuisine = "C", ImageUrl = "U", AuthorId = 1 };
            _efContext.Recipes.Add(recipe);
            _efContext.SaveChanges();

            _repository.Delete(10);
            _efContext.SaveChanges();

            _efContext.Recipes.Should().BeEmpty();
        }

        [Fact]
        public void Update_ShouldUpdateFields_And_Ingredients_Logic()
        {

            // 1. Arrange: Istniejący przepis ze starym składnikiem
            var existingRecipe = new Recipe
            {
                Id = 1,
                Title = "Old",
                Description = "D",
                Instructions = "I",
                Cuisine = "C",
                ImageUrl = "U",
                AuthorId = 1,
                RecipeIngredients = new List<RecipeIngredient>
                {
                    new RecipeIngredient { IngredientId = 100, Quantity = "100g" }
                }
            };
            _efContext.Recipes.Add(existingRecipe);
            _efContext.SaveChanges();

            // 2. Dane do aktualizacji (nowy tytuł + ZMIANA składników)
            var updateData = new Recipe
            {
                Id = 1,
                Title = "New Title",
                Description = "New D",
                Instructions = "New I",
                Cuisine = "New C",
                ImageUrl = "New U",
                AuthorId = 1,
                CategoryId = 5,
                PreparationTime = 20,
                CookTime = 30,
                Servings = 4,
                RecipeIngredients = new List<RecipeIngredient>
                {
                    new RecipeIngredient { IngredientId = 200, Quantity = "200g" } // Nowy składnik
                }
            };

            // Act
            _efContext.ChangeTracker.Clear();
            _repository.Update(updateData);
            _efContext.SaveChanges();

            // Assert
            var updated = _efContext.Recipes.Include(r => r.RecipeIngredients).First(r => r.Id == 1);

            updated.Title.Should().Be("New Title");
            // Sprawdzamy czy logika .Clear() i .Add() w repozytorium zadziałała
            updated.RecipeIngredients.Should().HaveCount(1);
            updated.RecipeIngredients.First().IngredientId.Should().Be(200);
        }

        [Fact]
        public void Update_ShouldReturnNull_WhenRecipeDoesNotExist()
        {
            var result = _repository.Update(new Recipe { Id = 999 });
            result.Should().BeNull();
        }
    }
}