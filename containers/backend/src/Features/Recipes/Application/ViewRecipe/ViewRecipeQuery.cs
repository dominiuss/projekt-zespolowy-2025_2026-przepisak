using MediatR;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;

namespace PrzepisakApi.src.Features.Recipes.Application.ViewRecipe;

    public class ViewRecipeQuery : IRequest<RecipeDTO>
    {
        public int Id { get; set; }
    }

