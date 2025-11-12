using MediatR;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;

namespace PrzepisakApi.src.Features.Recipes.Application.Search.SearchByRecipeTitle
{
    public class SearchByRecipeTitleQuery : IRequest<List<RecipeOverviewDTO>>
    {
        public string Title { get; set; }
    }
}
