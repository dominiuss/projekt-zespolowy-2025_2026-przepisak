using MediatR;
using PrzepisakApi.src.Features.Ratings.Application.DTOs;

namespace PrzepisakApi.src.Features.Ratings.Application.AddRating
{
    public class AddRatingCommand : IRequest<Unit>
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public int Score { get; set; }
        public string? Comment { get; set; }
    }
}