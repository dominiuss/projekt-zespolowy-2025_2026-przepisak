namespace PrzepisakApi.api.src.Features.Auth.Application.DTOs;

public class LoginResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
}
