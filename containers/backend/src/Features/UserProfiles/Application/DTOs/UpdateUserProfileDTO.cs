using System.Diagnostics.CodeAnalysis;

namespace PrzepisakApi.src.Features.UserProfiles.Application.DTOs
{
    [ExcludeFromCodeCoverage]
    public class UpdateProfileRequestDto
    {
        public string username { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
    }
}