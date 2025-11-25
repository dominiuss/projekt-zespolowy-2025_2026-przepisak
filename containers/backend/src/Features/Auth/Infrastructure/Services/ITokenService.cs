using Microsoft.AspNetCore.Identity;

namespace PrzepisakApi.api.src.Features.Auth.Services;

public interface ITokenService
{
    Task<TokenResponseDto> GenerateTokensAsync(IdentityUser user, int userProfileId);
    Task<TokenResponseDto?> RefreshTokensAsync(string identityId, string refreshToken);
    public string CreateAccessToken(IdentityUser user, int userProfileId);
}
