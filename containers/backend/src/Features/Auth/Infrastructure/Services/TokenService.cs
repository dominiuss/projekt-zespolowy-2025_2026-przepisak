//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using miejsce.api.src.Data;
//using miejsce.api.src.Features.Auth.Infrastructure.Options;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;

//namespace PrzepisakApi.api.src.Features.Auth.Services;

//public class TokenService : ITokenService
//{
//    private readonly UserManager<IdentityUser> _userManager;
//    private readonly IEfContext _context;
//    private readonly JwtSettings _jwtSettings;

//    public TokenService(UserManager<IdentityUser> userManager, IEfContext context, IOptions<JwtSettings> jwtSettings)
//    {
//        _userManager = userManager;
//        _context = context;
//        _jwtSettings = jwtSettings.Value;
//    }

//    public async Task<TokenResponseDto> GenerateTokensAsync(IdentityUser user, int userProfileId)
//    {
//        var accessToken = CreateAccessToken(user, userProfileId);
//        var refreshToken = await GenerateAndSaveRefreshTokenAsync(user);

//        return new TokenResponseDto
//        {
//            AccessToken = accessToken,
//            RefreshToken = refreshToken
//        };
//    }

//    public string CreateAccessToken(IdentityUser user, int userProfileId)
//    {
//        var claims = new List<Claim>
//        {
//            new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? ""),
//            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
//            new Claim("IdentityId", user.Id),
//            new Claim("UserProfileId", userProfileId.ToString())
//        };

//        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey));
//        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//        var token = new JwtSecurityToken(
//            issuer: _jwtSettings.Issuer,
//            audience: _jwtSettings.Audiences.First(),
//            claims: claims,
//            notBefore: DateTime.UtcNow,
//            expires: DateTime.UtcNow.AddHours(2),
//            signingCredentials: creds
//        );

//        return new JwtSecurityTokenHandler().WriteToken(token);
//    }

//    private string GenerateRefreshToken()
//    {
//        var randomBytes = new byte[32];
//        using var rng = RandomNumberGenerator.Create();
//        rng.GetBytes(randomBytes);
//        return Convert.ToBase64String(randomBytes);
//    }

//    private async Task<string> GenerateAndSaveRefreshTokenAsync(IdentityUser user)
//    {
//        var refreshToken = GenerateRefreshToken();
//        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(u => u.IdentityId == user.Id);

//        if (userProfile != null)
//        {
//            userProfile.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
//            await _context.SaveChangesAsync();
//        }

//        return refreshToken;
//    }

//    public async Task<TokenResponseDto?> RefreshTokensAsync(string identityId, string refreshToken)
//    {
//        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(u => u.IdentityId == identityId);

//        if (userProfile == null || !userProfile.ValidateRefreshToken(refreshToken))
//            return null;

//        var user = await _userManager.FindByIdAsync(userProfile.IdentityId);
//        if (user == null)
//            return null;

//        return await GenerateTokensAsync(user, userProfile.Id);
//    }
//}
