using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PrzepisakApi.api.src.Features.Auth.Infrastructure.Options;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Auth.Domain;
using PrzepisakApi.src.Features.UserProfile.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PrzepisakApi.api.src.Features.Auth.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly IUserRepository _userRepository;

    public TokenService(UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings, IUserRepository userRepository)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _userRepository = userRepository;
    }

    public async Task<TokenResponseDto> GenerateTokensAsync(IdentityUser user, int userProfileId)
    {
        var accessToken = CreateAccessToken(user, userProfileId);
        var refreshToken = await GenerateAndSaveRefreshTokenAsync(user);

        return new TokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public string CreateAccessToken(IdentityUser user, int userProfileId)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim("IdentityId", user.Id),
            new Claim("UserProfileId", userProfileId.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audiences.First(),
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private async Task<string> GenerateAndSaveRefreshTokenAsync(IdentityUser user)
    {
        var refreshToken = GenerateRefreshToken();
        await _userRepository.SetRefreshTokenAsync(user.Id,
        refreshToken,
        DateTime.UtcNow.AddDays(7));
        return refreshToken;
    }

    public async Task<TokenResponseDto?> RefreshTokensAsync(string identityId, string refreshToken)
    {
        var userProfile = await _userRepository.GetUserProfileWithRefreshTokenAsync(identityId);

        if (userProfile == null || !ValidateRefreshToken(userProfile,refreshToken))
            return null;

        var user = await _userManager.FindByIdAsync(userProfile.IdentityUserId);
        if (user == null)
            return null;

        return await GenerateTokensAsync(user, userProfile.Id);
    }
    public async Task SetRefreshTokenAsync(User userProfile, string refreshToken, DateTime expiration)
    {
        userProfile.RefreshToken.Token = refreshToken;
        userProfile.RefreshToken.RefreshTokenExpiration = expiration;
    }

    public bool ValidateRefreshToken(User userProfile, string refreshToken)
    {
        return userProfile.RefreshToken.Token == refreshToken &&
               userProfile.RefreshToken.RefreshTokenExpiration != null &&
               userProfile.RefreshToken.RefreshTokenExpiration > DateTime.UtcNow;
    }

}
