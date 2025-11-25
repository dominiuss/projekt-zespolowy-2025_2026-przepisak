using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrzepisakApi.src.Features.Auth.Domain;
using PrzepisakApi.src.Features.Ratings.Application.AddRating;
using PrzepisakApi.src.Features.Ratings.Application.DTOs;
using PrzepisakApi.src.Features.Ratings.Application.GetRecipeRatings;
using System.Security.Claims;

namespace PrzepisakApi.src.Features.Ratings.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;

        public RatingsController(IMediator mediator, IUserRepository userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddRating([FromBody] AddRatingDTO dto)
        {
            var identityIdClaim = User.Claims.FirstOrDefault(x => x.Type == "IdentityId")
                                  ?? User.FindFirst(ClaimTypes.NameIdentifier);

            if (identityIdClaim == null || string.IsNullOrEmpty(identityIdClaim.Value))
            {
                return Unauthorized(new { Message = "Błąd: Token nie zawiera IdentityId." });
            }
            var userProfile = await _userRepository.GetUserProfileAsync(identityIdClaim.Value);

            if (userProfile == null)
            {
                return Unauthorized(new { Message = "Nie znaleziono profilu użytkownika w bazie." });
            }

            var command = new AddRatingCommand
            {
                UserId = userProfile.Id,
                RecipeId = dto.RecipeId,
                Score = dto.Score,
                Comment = dto.Comment
            };

            await _mediator.Send(command);
            return Ok(new { Message = "Ocena dodana pomyślnie" });
        }

        [HttpGet("{recipeId}")]
        public async Task<ActionResult<List<RatingDTO>>> GetRatings([FromRoute] int recipeId)
        {
            var result = await _mediator.Send(new GetRecipeRatingsQuery { RecipeId = recipeId });
            return Ok(result);
        }
    }
}