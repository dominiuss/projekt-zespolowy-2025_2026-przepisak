using MediatR;
using Microsoft.AspNetCore.Mvc;
using PrzepisakApi.src.Features.UserProfile.Application.GetUserProfile;
using PrzepisakApi.src.Features.UserProfile.Application.UpdateUserProfile;
using PrzepisakApi.src.Features.UserProfiles.Application.DTOs;
using System.Security.Claims;

namespace PrzepisakApi.src.Features.UserProfile.Api
{
    [ApiController]
    [Route("api/user/profile")]
    public class UserProfilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserProfilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim is null)
                return 1; // na sztywno na testy

            return int.Parse(idClaim.Value);
        }


        [HttpGet]
        public async Task<ActionResult<UserProfileDTO>> Get()
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new GetUserProfileQuery { UserId = userId });

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<UserProfileDTO>> Update([FromBody] UpdateUserProfileCommand command)
        {
            var userId = GetCurrentUserId();
            command.UserId = userId;

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
