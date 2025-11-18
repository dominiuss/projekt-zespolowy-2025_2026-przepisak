using MediatR;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;

namespace PrzepisakApi.src.Features.Recipes.Application.ViewAllRecipes
{
    public class ViewAllRecipesQuery : IRequest<List<RecipeOverviewDTO>>
    {
    }
}
