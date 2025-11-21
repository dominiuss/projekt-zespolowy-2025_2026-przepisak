namespace PrzepisakApi.src.Features.UserProfiles.Application.DTOs
{
    public class UpdateProfileRequestDto
    {
        public string username { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
    }
}