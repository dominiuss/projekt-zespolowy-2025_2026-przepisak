namespace PrzepisakApi.api.src.Features.Auth.Services.DTOs;

public class TokenInternalResult
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
}
