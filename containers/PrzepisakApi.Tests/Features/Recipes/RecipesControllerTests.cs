using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PrzepisakApi.src.Features.Recipes.Api;
using PrzepisakApi.src.Features.Recipes.Application.AddRecipe;
using PrzepisakApi.src.Features.Recipes.Application.DeleteRecipe;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using PrzepisakApi.src.Features.Recipes.Application.UpdateRecipe;
using PrzepisakApi.src.Features.Recipes.Application.ViewAllRecipes;
using PrzepisakApi.src.Features.Recipes.Application.ViewRecipe;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PrzepisakApi.Tests.Features.Recipes
{
    public class RecipesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly RecipesController _controller;

        public RecipesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _mapperMock = new Mock<IMapper>();
            _controller = new RecipesController(_mediatorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllRecipes_ShouldReturnList()
        {
            // Arrange
            var query = new ViewAllRecipesQuery();
            var list = new List<RecipeOverviewDTO> { new RecipeOverviewDTO { Title = "Test" } };
            _mediatorMock.Setup(m => m.Send(query, default)).ReturnsAsync(list);

            // Act
            var result = await _controller.GetAllRecipes(query);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(list);
        }

        [Fact]
        public async Task GetRecipeById_ShouldReturnNotFound_WhenNull()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ViewRecipeQuery>(), default)).ReturnsAsync((RecipeDTO?)null);

            var result = await _controller.GetRecipeById(1);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Add_ShouldReturnCreated()
        {
            // Arrange
            var dto = new AddRecipeDTO { Title = "New" };
            var command = new AddRecipeCommand();
            var resultDto = new AddUpdateRecipeDTO { Id = 10, Title = "New" };

            _mapperMock.Setup(m => m.Map<AddRecipeCommand>(dto)).Returns(command);
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(resultDto);

            // Act
            var result = await _controller.Add(dto);

            // Assert
            var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.ActionName.Should().Be("GetRecipeById");
            createdResult.RouteValues["id"].Should().Be(10);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent()
        {
            // Act
            var result = await _controller.Delete(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mediatorMock.Verify(m => m.Send(It.Is<DeleteRecipeCommand>(c => c.Id == 1), default), Times.Once);
        }
    }
}