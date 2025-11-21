using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Auth.Domain;
using PrzepisakApi.src.Features.UserProfile.Domain;

namespace PrzepisakApi.src.Features.Auth.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEfContext _efContext;

        public UserRepository(UserManager<IdentityUser> userManager, IEfContext efContext)
        {
            _userManager = userManager;
            _efContext = efContext;
        }

        public async Task<IdentityUser> CreateUserAsync(string username, string password)
        {
            var user = new IdentityUser { UserName = username};
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return user;
        }

        public void AddUserProfile(User profile)
        {
            _efContext.Users.Add(profile);
        }

        public async Task SaveChangesAsync()
        {
            await _efContext.SaveChangesAsync();
        }

        public async Task<IdentityUser?> GetUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<User?> GetUserProfileAsync(string identityId)
        {
            return await _efContext.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityId);
        }

        public async Task SetRefreshTokenAsync(string identityId, string refreshToken, DateTime expiration)
        {
            var userProfile = await _efContext.Users
        .Include(u => u.RefreshToken)
        .FirstOrDefaultAsync(u => u.IdentityUserId == identityId);

            if (userProfile?.RefreshToken != null)
            {
                userProfile.RefreshToken.Token = refreshToken;
                userProfile.RefreshToken.RefreshTokenExpiration = expiration;
                await _efContext.SaveChangesAsync();
            }
        }

        public async Task<User?> GetUserProfileWithRefreshTokenAsync(string identityId)
        {
            return await _efContext.Users
                .Include(u => u.RefreshToken)
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityId);
        }
    }
}
