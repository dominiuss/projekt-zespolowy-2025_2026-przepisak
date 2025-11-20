using Microsoft.AspNetCore.Identity;
using PrzepisakApi.src.Features.UserProfile.Domain;

namespace PrzepisakApi.src.Features.Auth.Domain
{
    public interface IUserRepository
    {
        Task<IdentityUser?> GetUserByUsernameAsync(string username);
        Task<bool> CheckPasswordAsync(IdentityUser user, string password);
        Task<User?> GetUserProfileAsync(string identityId);
        Task<IdentityUser> CreateUserAsync(string username, string password);
        Task SetRefreshTokenAsync(string identityId, string refreshToken, DateTime expiration);
        Task<User?> GetUserProfileWithRefreshTokenAsync(string identityId);
        void AddUserProfile(User profile);
        Task SaveChangesAsync();
    }
}
