using System.Diagnostics.CodeAnalysis;

namespace PrzepisakApi.api.src.Features.Auth.Application.DTOs;
[ExcludeFromCodeCoverage]
public class LoginResponse
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? Expiration { get; set; }
    public string? ErrorMessage { get; set; }
}
