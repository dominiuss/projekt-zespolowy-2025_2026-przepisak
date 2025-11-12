using MediatR;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;

namespace PrzepisakApi.src.Features.Recipes.Application.Search.SearchByAuthorName
{
    public class SearchByAuthorNameQuery : IRequest<List<RecipeOverviewDTO>>
    {
        public string AuthorName { get; set; }
    }
}
