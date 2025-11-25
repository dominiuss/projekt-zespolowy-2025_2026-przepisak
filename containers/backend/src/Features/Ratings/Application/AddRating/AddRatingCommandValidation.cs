using FluentValidation;

namespace PrzepisakApi.src.Features.Ratings.Application.AddRating
{
    public class AddRatingCommandValidation : AbstractValidator<AddRatingCommand>
    {
        public AddRatingCommandValidation()
        {
            RuleFor(x => x.RecipeId)
                .GreaterThan(0);

            RuleFor(x => x.Score)
                .InclusiveBetween(1, 5)
                .WithMessage("Ocena musi być w przedziale 1-5.");

            RuleFor(x => x.Comment)
                .MaximumLength(500)
                .WithMessage("Komentarz nie może być dłuższy niż 500 znaków.");
        }
    }
}