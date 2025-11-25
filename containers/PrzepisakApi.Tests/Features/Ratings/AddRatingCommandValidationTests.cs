using FluentValidation.TestHelper;
using PrzepisakApi.src.Features.Ratings.Application.AddRating;

namespace PrzepisakApi.Tests.Features.Ratings
{
    public class AddRatingCommandValidationTests
    {
        private readonly AddRatingCommandValidation _validator;

        public AddRatingCommandValidationTests()
        {
            _validator = new AddRatingCommandValidation();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        public void ShouldHaveError_WhenScoreIsOutOfRange(int score)
        {
            var command = new AddRatingCommand { Score = score, RecipeId = 1 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Score);
        }

        [Fact]
        public void ShouldNotHaveError_WhenScoreIsValid()
        {
            var command = new AddRatingCommand { Score = 5, RecipeId = 1 };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Score);
        }

        [Fact]
        public void ShouldHaveError_WhenCommentIsTooLong()
        {
            var command = new AddRatingCommand { Score = 5, RecipeId = 1, Comment = new string('a', 501) };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Comment);
        }
    }
}