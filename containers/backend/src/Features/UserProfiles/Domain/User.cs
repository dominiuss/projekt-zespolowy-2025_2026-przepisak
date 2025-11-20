using Microsoft.AspNetCore.Identity;
using PrzepisakApi.src.Features.UserProfiles.Domain;

namespace PrzepisakApi.src.Features.UserProfile.Domain
{
    public class User
    {
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; } = null!;
        public int Id { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
        public RefreshToken RefreshToken { get; set; } = null!;
    }
}
