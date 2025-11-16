namespace PrzepisakApi.api.src.Features.Auth.Infrastructure.Options;

public class JwtSettings
{
    public string SigningKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string[] Audiences { get; set; } = Array.Empty<string>();
}
