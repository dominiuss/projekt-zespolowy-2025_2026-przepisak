using MediatR;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;

namespace PrzepisakApi.src.Features.Recipes.Application.ViewAllRecipes
{
    public class ViewAllRecipesQuery : IRequest<List<RecipeOverviewDTO>>
    {
        public List<int>? CategoryIds { get; set; }
        public List<int>? IncludeIngredientIds { get; set; }
        public List<int>? ExcludeIngredientIds { get; set; }
    }
}