using Microsoft.AspNetCore.Identity;
using PrzepisakApi.src.Features.UserProfile.Domain;

namespace PrzepisakApi.src.Features.Auth.Domain
{
    public interface IUserRepository
    {
        Task<IdentityUser> CreateUserAsync(string username, string password);
        void AddUserProfile(User profile);
        Task SaveChangesAsync();
    }
}
