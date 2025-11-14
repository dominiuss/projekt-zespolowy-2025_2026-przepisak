using Microsoft.EntityFrameworkCore;
using PrzepisakApi.src.Features.Recipes.Domain;
using PrzepisakApi.src.Features.UserProfile.Domain;

namespace PrzepisakApi.src.Database
{
    public class EfContext : DbContext, IEfContext
    {
        public EfContext(DbContextOptions<EfContext> options) : base(options)
        {
        }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfContext).Assembly);
        }
        public void SeedData()
        {
            if (!Users.Any())
            {
                Users.Add(new User { Id = 1, Username = "admin" });
                Users.Add(new User { Id = 2, Username = "chef" });
            }

            if (!Categories.Any())
            {
                Categories.Add(new Category { Id = 1, Name = "Dessert", ParentCategoryId = null });
                Categories.Add(new Category { Id = 2, Name = "Main Course", ParentCategoryId = null });
            }

            if (!Recipes.Any())
            {
                Recipes.Add(new Recipe
                {
                    Title = "Chocolate Cake",
                    AuthorId = 1,
                    Description = "Delicious chocolate cake",
                    Instructions = "Mix ingredients and bake",
                    PreparationTime = 30,
                    CookTime = 45,
                    Servings = 8,
                    CategoryId = 1,
                    Cuisine = "International",
                    ImageUrl = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            SaveChanges();
        }
    }
}