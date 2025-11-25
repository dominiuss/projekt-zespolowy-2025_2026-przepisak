using System.Diagnostics.CodeAnalysis;

namespace PrzepisakApi.src.Features.UserProfiles.Application.DTOs
{
    [ExcludeFromCodeCoverage]
    public class UserProfileDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
