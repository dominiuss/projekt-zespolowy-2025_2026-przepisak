using MediatR;
using PrzepisakApi.src.Features.Ratings.Application.DTOs;

namespace PrzepisakApi.src.Features.Ratings.Application.GetRecipeRatings
{
    public class GetRecipeRatingsQuery : IRequest<List<RatingDTO>>
    {
        public int RecipeId { get; set; }
    }
}