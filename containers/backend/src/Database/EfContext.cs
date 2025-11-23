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
                // A. Tworzenie kont Identity (Logowanie)
                var adminIdentity = new IdentityUser { UserName = "admin", Email = "admin@test.com" };
                var chefIdentity = new IdentityUser { UserName = "chef", Email = "chef@test.com" };

                if (await userManager.FindByNameAsync("admin") == null)
                    await userManager.CreateAsync(adminIdentity, "Password123!");

                if (await userManager.FindByNameAsync("chef") == null)
                    await userManager.CreateAsync(chefIdentity, "Password123!");

                // Pobierz utworzone IdentityUser, aby mieć ich GUID
                var adminCreated = await userManager.FindByNameAsync("admin");
                var chefCreated = await userManager.FindByNameAsync("chef");

                // B. Tworzenie encji domenowych User (Profil w aplikacji)
                // To jest krok, którego brakowało!
                var usersList = new List<User>
                {
                    new User
                    {
                        IdentityUserId = adminCreated.Id, // Łączymy kluczem obcym
                        // Ustaw inne wymagane pola User, jeśli istnieją
                    },
                    new User
                    {
                        IdentityUserId = chefCreated.Id,
                    }
                };

                Users.AddRange(usersList);
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
                // Teraz to zadziała, bo Users.FirstAsync znajdzie rekordy dodane w kroku 1
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

                Recipes.Add(new Recipe
                {
                    Title = "Chocolate Cake",
                    AuthorId = adminUser.Id, // Tu był błąd: adminUser nie istniał w tabeli Users
                    Description = "Delicious chocolate cake",
                    Instructions = "Mix ingredients and bake",
                    PreparationTime = 30,
                    CookTime = 45,
                    Servings = 8,
                    CategoryId = catDessert.Id,
                    Cuisine = "International",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/0/04/Pound_layer_cake.jpg", // Dodaj przykładowe URL
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
                });

                // ... Pozostałe przepisy bez zmian (skopiuj z poprzedniej odpowiedzi) ...
                // Dla czytelności skróciłem tutaj kod, ale wklej tu resztę przepisów.

                // Przykład drugiego dla pewności:
                Recipes.Add(new Recipe
                {
                    Title = "Spaghetti Bolognese",
                    AuthorId = chefUser.Id,
                    Description = "Classic Italian pasta",
                    Instructions = "Cook pasta...",
                    PreparationTime = 20,
                    CookTime = 40,
                    Servings = 4,
                    CategoryId = catMain.Id,
                    Cuisine = "Italian",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/2a/Spaghetti_al_Pomodoro.JPG",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    RecipeIngredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient { Ingredient = pasta, Quantity = "500g" },
                        new RecipeIngredient { Ingredient = beef, Quantity = "400g" },
                        new RecipeIngredient { Ingredient = tomatoes, Quantity = "1 can" },
                        new RecipeIngredient { Ingredient = onion, Quantity = "1 pc" }
                    }
                });

                await SaveChangesAsync();
            }
        }
    }
}