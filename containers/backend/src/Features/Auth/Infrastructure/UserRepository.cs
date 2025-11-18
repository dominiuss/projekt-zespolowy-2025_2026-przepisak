using Microsoft.AspNetCore.Identity;
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
    }
}
