namespace PrzepisakApi.api.src.Features.Auth.Services
{
    public class TokenResponseDto
    {
        public required string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
