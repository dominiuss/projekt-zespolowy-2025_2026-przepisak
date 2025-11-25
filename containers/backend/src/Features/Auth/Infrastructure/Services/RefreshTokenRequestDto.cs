namespace PrzepisakApi.api.src.Features.Auth.Services
{
    public class RefreshTokenRequestDto
    {
        public Guid UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}
