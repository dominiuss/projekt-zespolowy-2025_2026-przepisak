using MediatR;
using PrzepisakApi.src.Features.UserProfiles.Application.DTOs;

namespace PrzepisakApi.src.Features.UserProfile.Application.GetUserProfile
{
    public class GetUserProfileQuery : IRequest<UserProfileDTO?>
    {
        public int UserId { get; set; }
    }
}
