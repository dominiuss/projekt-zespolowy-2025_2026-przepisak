using Microsoft.EntityFrameworkCore;
using PrzepisakApi.src.Features.Recipe.Domain;

namespace PrzepisakApi.src.Database
{
    public interface IPrzepisakDbContext
    {
        DbSet<Recipe> Recipes { get; set; }
        DbSet<Category> Categories { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
