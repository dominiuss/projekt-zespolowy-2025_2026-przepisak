using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrzepisakApi.src.Features.Recipes.Domain;
using PrzepisakApi.src.Features.UserProfile.Domain;

namespace PrzepisakApi.src.Database
{
    public class EfContext : IdentityDbContext<IdentityUser>, IEfContext
    {
        public EfContext(DbContextOptions<EfContext> options) : base(options)
        {
        }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfContext).Assembly);
        }
        public void SeedData(UserManager<IdentityUser> userManager)
        {
            if (!Users.Any())
            {
                var adminIdentity = new IdentityUser { UserName = "admin"};
                var chefIdentity = new IdentityUser { UserName = "chef"};

                userManager.CreateAsync(adminIdentity, "Password123!").Wait();
                userManager.CreateAsync(chefIdentity, "Password123!").Wait();

                Users.Add(new User
                {
                    IdentityUserId = adminIdentity.Id
                });

                Users.Add(new User
                {
                    IdentityUserId = chefIdentity.Id
                });

                SaveChanges();
            }

            if (!Categories.Any())
            {
                Categories.Add(new Category { Id = 1, Name = "Dessert", ParentCategoryId = null });
                Categories.Add(new Category { Id = 2, Name = "Main Course", ParentCategoryId = null });
                SaveChanges();
            }

            if (!Recipes.Any())
            {
                var adminUser = Users.First(u => u.IdentityUserId ==
                    userManager.Users.First(x => x.UserName == "admin").Id);
                Recipes.Add(new Recipe
                {
                    Title = "Chocolate Cake",
                    AuthorId = adminUser.Id,
                    Description = "Delicious chocolate cake",
                    Instructions = "Mix ingredients and bake",
                    PreparationTime = 30,
                    CookTime = 45,
                    Servings = 8,
                    CategoryId = Categories.First(c => c.Name == "Dessert").Id,
                    Cuisine = "International",
                    ImageUrl = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

                SaveChanges();
            }
        }

    }
}