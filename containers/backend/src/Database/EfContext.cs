using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrzepisakApi.src.Features.Recipes.Domain;
using PrzepisakApi.src.Features.UserProfile.Domain;
using PrzepisakApi.src.Features.Ratings.Domain;

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
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfContext).Assembly);
        }

        public async Task SeedData(UserManager<IdentityUser> userManager)
        {
            if (!Users.Any())
            {
                var adminIdentity = new IdentityUser { UserName = "admin" };
                var chefIdentity = new IdentityUser { UserName = "chef" };

                if (await userManager.FindByNameAsync("admin") == null)
                    await userManager.CreateAsync(adminIdentity, "Password123!");

                if (await userManager.FindByNameAsync("chef") == null)
                    await userManager.CreateAsync(chefIdentity, "Password123!");

                var adminCreated = await userManager.FindByNameAsync("admin");
                var chefCreated = await userManager.FindByNameAsync("chef");

                var adminProfile = new User
                {
                    IdentityUserId = adminCreated.Id,
                    Bio = "Admin",
                    AvatarUrl = ""
                };
                Users.Add(adminProfile);

                var chefProfile = new User
                {
                    IdentityUserId = chefCreated.Id,
                    Bio = "Chef",
                    AvatarUrl = ""
                };
                Users.Add(chefProfile);

                await SaveChangesAsync();
            }

            if (!Categories.Any())
            {
                Categories.Add(new Category { Name = "Dessert", ParentCategoryId = null });
                Categories.Add(new Category { Name = "Main Course", ParentCategoryId = null });
                Categories.Add(new Category { Name = "Breakfast", ParentCategoryId = null });
                Categories.Add(new Category { Name = "Soup", ParentCategoryId = null });
                Categories.Add(new Category { Name = "Vegetarian", ParentCategoryId = null });
                await SaveChangesAsync();
            }

            if (!Recipes.Any())
            {
                var adminUser = await Users.FirstAsync(u => u.IdentityUser.UserName == "admin");
                var chefUser = await Users.FirstAsync(u => u.IdentityUser.UserName == "chef");

                var catDessert = await Categories.FirstAsync(c => c.Name == "Dessert");
                var catMain = await Categories.FirstAsync(c => c.Name == "Main Course");
                var catBreakfast = await Categories.FirstAsync(c => c.Name == "Breakfast");
                var catSoup = await Categories.FirstAsync(c => c.Name == "Soup");
                var catVege = await Categories.FirstAsync(c => c.Name == "Vegetarian");

                Recipes.Add(new Recipe
                {
                    Title = "Chocolate Cake",
                    AuthorId = adminUser.Id,
                    Description = "Delicious chocolate cake",
                    Instructions = "Mix ingredients and bake",
                    PreparationTime = 30,
                    CookTime = 45,
                    Servings = 8,
                    CategoryId = catDessert.Id,
                    Cuisine = "International",
                    ImageUrl = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

                Recipes.Add(new Recipe
                {
                    Title = "Spaghetti Bolognese",
                    AuthorId = chefUser.Id,
                    Description = "Classic Italian pasta with meat sauce",
                    Instructions = "Cook pasta. Prepare sauce with beef and tomatoes. Mix.",
                    PreparationTime = 20,
                    CookTime = 40,
                    Servings = 4,
                    CategoryId = catMain.Id,
                    Cuisine = "Italian",
                    ImageUrl = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

                Recipes.Add(new Recipe
                {
                    Title = "Tomato Soup",
                    AuthorId = chefUser.Id,
                    Description = "Creamy tomato soup with basil",
                    Instructions = "Roast tomatoes. Blend with broth and cream.",
                    PreparationTime = 10,
                    CookTime = 30,
                    Servings = 4,
                    CategoryId = catSoup.Id,
                    Cuisine = "International",
                    ImageUrl = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

                Recipes.Add(new Recipe
                {
                    Title = "Scrambled Eggs",
                    AuthorId = adminUser.Id,
                    Description = "Fluffy eggs with chives",
                    Instructions = "Whisk eggs. Fry on butter. Add salt.",
                    PreparationTime = 5,
                    CookTime = 5,
                    Servings = 1,
                    CategoryId = catBreakfast.Id,
                    Cuisine = "Polish",
                    ImageUrl = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

                Recipes.Add(new Recipe
                {
                    Title = "Grilled Tofu Salad",
                    AuthorId = chefUser.Id,
                    Description = "Healthy salad with tofu and veggies",
                    Instructions = "Grill tofu. Chop veggies. Mix with dressing.",
                    PreparationTime = 15,
                    CookTime = 10,
                    Servings = 2,
                    CategoryId = catVege.Id,
                    Cuisine = "Vegan",
                    ImageUrl = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

                await SaveChangesAsync();
            }
        }
    }
}