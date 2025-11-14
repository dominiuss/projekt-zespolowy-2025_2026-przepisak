using MediatR;
using PrzepisakApi.src.Features.UserProfiles.Application.DTOs;

namespace PrzepisakApi.src.Features.UserProfile.Application.UpdateUserProfile
{
    public class UpdateUserProfileCommand : IRequest<UserProfileDTO>
    {
        public int UserId { get; set; }
        public string Username { get; set; }
    }
}
