using MediatR;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Ratings.Domain;

namespace PrzepisakApi.src.Features.Ratings.Application.AddRating
{
    public class AddRatingCommandHandler : IRequestHandler<AddRatingCommand, Unit>
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IEfContext _efContext;

        public AddRatingCommandHandler(IRatingRepository ratingRepository, IEfContext efContext)
        {
            _ratingRepository = ratingRepository;
            _efContext = efContext;
        }

        public async Task<Unit> Handle(AddRatingCommand request, CancellationToken cancellationToken)
        {
            var existingRating = await _ratingRepository.GetUserRatingForRecipeAsync(request.UserId, request.RecipeId);

            if (existingRating != null)
            {
                existingRating.Score = request.Score;
                existingRating.Comment = request.Comment;
                existingRating.CreatedAt = DateTime.UtcNow;
            }
            else
            {
                var rating = new Rating
                {
                    UserId = request.UserId,
                    RecipeId = request.RecipeId,
                    Score = request.Score,
                    Comment = request.Comment,
                    CreatedAt = DateTime.UtcNow
                };
                await _ratingRepository.AddAsync(rating);
            }

            await _efContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}