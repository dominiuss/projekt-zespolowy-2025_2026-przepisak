using Microsoft.EntityFrameworkCore;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.UserProfile.Domain;

namespace PrzepisakApi.src.Features.UserProfile.Infrastructure
{
    public sealed class UserProfileRepository : IUserProfileRepository
    {
        private readonly IEfContext _db;

        public UserProfileRepository(IEfContext db)
        {
            _db = db;
        }

        public async Task<User?> GetByUserIdAsync(int userId, CancellationToken ct = default)
        {
            return await _db.Users
                .Include(x => x.IdentityUser)
                .FirstOrDefaultAsync(x => x.Id == userId, ct);
        }

        public async Task<User> CreateForUserAsync(int userId, CancellationToken ct = default)
        {
            var user = new User { Id = userId };
            await _db.Users.AddAsync(user, ct);
            return user;
        }

        public async Task SaveAsync(CancellationToken ct = default)
        {
            await _db.SaveChangesAsync(ct);
        }
    }
}