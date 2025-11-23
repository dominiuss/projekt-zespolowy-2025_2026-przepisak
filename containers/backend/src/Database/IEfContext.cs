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
<<<<<<< HEAD
        DbSet<Rating> Ratings { get; set; }
=======
        DbSet<Ingredient> Ingredients { get; set; }
>>>>>>> f627f48032be820809713083c5a7e22687c44da2

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
