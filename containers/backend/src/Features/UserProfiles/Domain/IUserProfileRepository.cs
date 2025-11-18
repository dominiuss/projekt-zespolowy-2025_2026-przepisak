namespace PrzepisakApi.src.Features.UserProfile.Domain
{
    public interface IUserProfileRepository
    {
        Task<User?> GetByUserIdAsync(int userId, CancellationToken ct = default);
        Task<User> CreateForUserAsync(int userId, CancellationToken ct = default);
        Task SaveAsync(CancellationToken ct = default);
    }
}
