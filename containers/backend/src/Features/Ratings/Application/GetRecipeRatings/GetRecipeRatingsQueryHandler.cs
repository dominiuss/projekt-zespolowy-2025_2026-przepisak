using MediatR;
using PrzepisakApi.src.Features.Ratings.Domain;
using PrzepisakApi.src.Features.Ratings.Application.DTOs;

namespace PrzepisakApi.src.Features.Ratings.Application.GetRecipeRatings
{
    public class GetRecipeRatingsQueryHandler : IRequestHandler<GetRecipeRatingsQuery, List<RatingDTO>>
    {
        private readonly IRatingRepository _ratingRepository;

        public GetRecipeRatingsQueryHandler(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<List<RatingDTO>> Handle(GetRecipeRatingsQuery request, CancellationToken cancellationToken)
        {
            return await _ratingRepository.GetRatingsByRecipeIdAsync(request.RecipeId);
        }
    }
}