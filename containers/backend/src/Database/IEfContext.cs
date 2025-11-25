using Microsoft.EntityFrameworkCore;
using PrzepisakApi.src.Features.Recipes.Domain;
using PrzepisakApi.src.Features.UserProfile.Domain;
using PrzepisakApi.src.Features.Ratings.Domain;

namespace PrzepisakApi.src.Database
{

    public interface IEfContext
    {
        DbSet<Recipe> Recipes { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Rating> Ratings { get; set; }
        DbSet<Ingredient> Ingredients { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
