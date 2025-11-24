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
        public new DbSet<User> Users { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfContext).Assembly);
        }

        public async Task SeedData(UserManager<IdentityUser> userManager)
        {
            // 1. NAPRAWA: Seedowanie użytkowników domenowych (User)
            if (!Users.Any())
            {
                var adminIdentity = new IdentityUser { UserName = "admin", Email = "admin@test.com" };
                var chefIdentity = new IdentityUser { UserName = "chef", Email = "chef@test.com" };

                if (await userManager.FindByNameAsync("admin") == null)
                    await userManager.CreateAsync(adminIdentity, "Password123!");

                if (await userManager.FindByNameAsync("chef") == null)
                    await userManager.CreateAsync(chefIdentity, "Password123!");

                // Pobierz utworzone IdentityUser, aby mieć ich GUID
                var adminCreated = await userManager.FindByNameAsync("admin");
                var chefCreated = await userManager.FindByNameAsync("chef");

                var usersList = new List<User>
                {
                    new User
                    {
                        IdentityUserId = adminCreated.Id,
                    },
                    new User
                    {
                        IdentityUserId = chefCreated.Id,
                    }
                };

                Users.AddRange(usersList);
                await SaveChangesAsync();
            }

            if (!Ingredients.Any())
            {
                Ingredients.AddRange(new List<Ingredient>
                {
                    new Ingredient { Name = "Flour" },
                    new Ingredient { Name = "Sugar" },
                    new Ingredient { Name = "Cocoa Powder" },
                    new Ingredient { Name = "Eggs" },
                    new Ingredient { Name = "Milk" },
                    new Ingredient { Name = "Pasta" },
                    new Ingredient { Name = "Ground Beef" },
                    new Ingredient { Name = "Tomatoes" },
                    new Ingredient { Name = "Onion" },
                    new Ingredient { Name = "Garlic" },
                    new Ingredient { Name = "Heavy Cream" },
                    new Ingredient { Name = "Basil" },
                    new Ingredient { Name = "Butter" },
                    new Ingredient { Name = "Chives" },
                    new Ingredient { Name = "Tofu" },
                    new Ingredient { Name = "Lettuce" },
                    new Ingredient { Name = "Cucumber" }
                });
                await SaveChangesAsync();
            }

            // 2. Seedowanie Składników (Bez zmian, działało poprawnie)
            if (!Ingredients.Any())
            {
                Ingredients.AddRange(new List<Ingredient>
                {
                    new Ingredient { Name = "Flour" },
                    new Ingredient { Name = "Sugar" },
                    new Ingredient { Name = "Cocoa Powder" },
                    new Ingredient { Name = "Eggs" },
                    new Ingredient { Name = "Milk" },
                    new Ingredient { Name = "Pasta" },
                    new Ingredient { Name = "Ground Beef" },
                    new Ingredient { Name = "Tomatoes" },
                    new Ingredient { Name = "Onion" },
                    new Ingredient { Name = "Garlic" },
                    new Ingredient { Name = "Heavy Cream" },
                    new Ingredient { Name = "Basil" },
                    new Ingredient { Name = "Butter" },
                    new Ingredient { Name = "Chives" },
                    new Ingredient { Name = "Tofu" },
                    new Ingredient { Name = "Lettuce" },
                    new Ingredient { Name = "Cucumber" }
                });
                await SaveChangesAsync();
            }

            // 3. Seedowanie Kategorii (Bez zmian)
            if (!Categories.Any())
            {
                Categories.Add(new Category { Name = "Dessert", ParentCategoryId = null });
                Categories.Add(new Category { Name = "Main Course", ParentCategoryId = null });
                Categories.Add(new Category { Name = "Breakfast", ParentCategoryId = null });
                Categories.Add(new Category { Name = "Soup", ParentCategoryId = null });
                Categories.Add(new Category { Name = "Vegetarian", ParentCategoryId = null });
                await SaveChangesAsync();
            }

            // 4. Seedowanie Przepisów
            if (!Recipes.Any())
            {
                var adminUser = await Users.Include(u => u.IdentityUser).FirstAsync(u => u.IdentityUser.UserName == "admin");
                var chefUser = await Users.Include(u => u.IdentityUser).FirstAsync(u => u.IdentityUser.UserName == "chef");

                var catDessert = await Categories.FirstAsync(c => c.Name == "Dessert");
                var catMain = await Categories.FirstAsync(c => c.Name == "Main Course");
                var catBreakfast = await Categories.FirstAsync(c => c.Name == "Breakfast");
                var catSoup = await Categories.FirstAsync(c => c.Name == "Soup");
                var catVege = await Categories.FirstAsync(c => c.Name == "Vegetarian");

                var flour = await Ingredients.FirstAsync(i => i.Name == "Flour");
                var sugar = await Ingredients.FirstAsync(i => i.Name == "Sugar");
                var cocoa = await Ingredients.FirstAsync(i => i.Name == "Cocoa Powder");
                var eggs = await Ingredients.FirstAsync(i => i.Name == "Eggs");
                var milk = await Ingredients.FirstAsync(i => i.Name == "Milk");
                var pasta = await Ingredients.FirstAsync(i => i.Name == "Pasta");
                var beef = await Ingredients.FirstAsync(i => i.Name == "Ground Beef");
                var tomatoes = await Ingredients.FirstAsync(i => i.Name == "Tomatoes");
                var onion = await Ingredients.FirstAsync(i => i.Name == "Onion");
                var garlic = await Ingredients.FirstAsync(i => i.Name == "Garlic");
                var cream = await Ingredients.FirstAsync(i => i.Name == "Heavy Cream");
                var basil = await Ingredients.FirstAsync(i => i.Name == "Basil");
                var butter = await Ingredients.FirstAsync(i => i.Name == "Butter");
                var chives = await Ingredients.FirstAsync(i => i.Name == "Chives");
                var tofu = await Ingredients.FirstAsync(i => i.Name == "Tofu");
                var lettuce = await Ingredients.FirstAsync(i => i.Name == "Lettuce");
                var cucumber = await Ingredients.FirstAsync(i => i.Name == "Cucumber");

                var recipesToAdd = new List<Recipe>
                {
                    new Recipe
                    {
                        Title = "Chocolate Cake",
                        AuthorId = adminUser.Id,
                        Description = "Delicious and moist chocolate cake perfect for birthdays.",
                        Instructions = "1. Mix dry ingredients. 2. Add wet ingredients. 3. Bake at 180C for 45 mins.",
                        PreparationTime = 30,
                        CookTime = 45,
                        Servings = 8,
                        CategoryId = catDessert.Id,
                        Cuisine = "International",
                        ImageUrl = "https://sallysbakingaddiction.com/wp-content/uploads/2013/04/triple-chocolate-cake-4.jpg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = flour, Quantity = "2 cups" },
                            new RecipeIngredient { Ingredient = sugar, Quantity = "1.5 cups" },
                            new RecipeIngredient { Ingredient = cocoa, Quantity = "0.5 cup" },
                            new RecipeIngredient { Ingredient = eggs, Quantity = "2 pcs" },
                            new RecipeIngredient { Ingredient = milk, Quantity = "1 cup" }
                        }
                    },

                    new Recipe
                    {
                        Title = "Spaghetti Bolognese",
                        AuthorId = chefUser.Id,
                        Description = "Classic Italian pasta with rich meat sauce.",
                        Instructions = "1. Cook beef with onions. 2. Add tomatoes and simmer. 3. Serve over boiled pasta.",
                        PreparationTime = 20,
                        CookTime = 40,
                        Servings = 4,
                        CategoryId = catMain.Id,
                        Cuisine = "Italian",
                        ImageUrl = "https://az.przepisy.pl/www-przepisy-pl/www.przepisy.pl/przepisy3ii/img/variants/800x0/proste-spaghetti-bolognese.jpg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = pasta, Quantity = "500g" },
                            new RecipeIngredient { Ingredient = beef, Quantity = "400g" },
                            new RecipeIngredient { Ingredient = tomatoes, Quantity = "1 can" },
                            new RecipeIngredient { Ingredient = onion, Quantity = "1 pc" },
                            new RecipeIngredient { Ingredient = garlic, Quantity = "2 cloves" }
                        }
                    },

                    new Recipe
                    {
                        Title = "Creamy Tomato Soup",
                        AuthorId = adminUser.Id,
                        Description = "Warming soup made with fresh tomatoes and cream.",
                        Instructions = "1. Sauté onions and garlic. 2. Add tomatoes and cook. 3. Blend and add cream.",
                        PreparationTime = 15,
                        CookTime = 25,
                        Servings = 4,
                        CategoryId = catSoup.Id,
                        Cuisine = "French",
                        ImageUrl = "https://www.happyfoodstube.com/wp-content/uploads/2020/03/creamy-tomato-basil-soup-image.jpg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = tomatoes, Quantity = "1kg" },
                            new RecipeIngredient { Ingredient = onion, Quantity = "1 pc" },
                            new RecipeIngredient { Ingredient = garlic, Quantity = "2 cloves" },
                            new RecipeIngredient { Ingredient = cream, Quantity = "100ml" },
                            new RecipeIngredient { Ingredient = basil, Quantity = "Fresh leaves" }
                        }
                    },

                    new Recipe
                    {
                        Title = "Fluffy Scrambled Eggs",
                        AuthorId = chefUser.Id,
                        Description = "Perfect breakfast eggs with chives.",
                        Instructions = "1. Whisk eggs with milk. 2. Melt butter on pan. 3. Cook gently and top with chives.",
                        PreparationTime = 5,
                        CookTime = 5,
                        Servings = 2,
                        CategoryId = catBreakfast.Id,
                        Cuisine = "American",
                        ImageUrl = "https://healthyrecipesblogs.com/wp-content/uploads/2025/01/fluffy-scrambled-eggs-featured-new-2024.jpg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = eggs, Quantity = "4 pcs" },
                            new RecipeIngredient { Ingredient = milk, Quantity = "2 tbsp" },
                            new RecipeIngredient { Ingredient = butter, Quantity = "1 tbsp" },
                            new RecipeIngredient { Ingredient = chives, Quantity = "Chopped" }
                        }
                    },

                    new Recipe
                    {
                        Title = "Fresh Tofu Salad",
                        AuthorId = adminUser.Id,
                        Description = "Healthy and crisp salad with marinated tofu.",
                        Instructions = "1. Cube tofu. 2. Chop vegetables. 3. Toss everything together.",
                        PreparationTime = 15,
                        CookTime = 0,
                        Servings = 2,
                        CategoryId = catVege.Id,
                        Cuisine = "Asian Fusion",
                        ImageUrl = "https://www.myplantifulcooking.com/wp-content/uploads/2022/04/japanese-tofu-salad-featured.jpg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = tofu, Quantity = "200g" },
                            new RecipeIngredient { Ingredient = lettuce, Quantity = "1 head" },
                            new RecipeIngredient { Ingredient = cucumber, Quantity = "1 pc" },
                            new RecipeIngredient { Ingredient = onion, Quantity = "0.5 pc" }
                        }
                    },

                    new Recipe
                    {
                        Title = "Garlic Butter Pasta",
                        AuthorId = chefUser.Id,
                        Description = "Simple yet delicious pasta dish.",
                        Instructions = "1. Boil pasta. 2. Sauté garlic in butter. 3. Mix pasta with sauce and herbs.",
                        PreparationTime = 10,
                        CookTime = 15,
                        Servings = 2,
                        CategoryId = catMain.Id,
                        Cuisine = "Italian",
                        ImageUrl = "https://tarasmulticulturaltable.com/wp-content/uploads/2019/04/Pasta-with-Garlic-Butter-Sauce-4-of-4.jpg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { Ingredient = pasta, Quantity = "300g" },
                            new RecipeIngredient { Ingredient = butter, Quantity = "50g" },
                            new RecipeIngredient { Ingredient = garlic, Quantity = "3 cloves" },
                            new RecipeIngredient { Ingredient = basil, Quantity = "Handful" }
                        }
                    }
                };

                Recipes.AddRange(recipesToAdd);
                await SaveChangesAsync();
            }
        }
    }
}