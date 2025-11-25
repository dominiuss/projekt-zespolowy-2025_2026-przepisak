using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Recipes.Application.UpdateRecipe;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using PrzepisakApi.src.Features.Recipes.Domain;
using PrzepisakApi.src.Features.UserProfile.Domain;
using System.Security.Claims;

namespace PrzepisakApi.Tests.Features.Recipes
{
    public class UpdateRecipeCommandHandlerTests
    {
        private readonly Mock<IRecipeRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IHttpContextAccessor> _httpContextMock;
        private readonly EfContext _efContext;
        private readonly UpdateRecipeCommandHandler _handler;

        public UpdateRecipeCommandHandlerTests()
        {
            _repoMock = new Mock<IRecipeRepository>();
            _mapperMock = new Mock<IMapper>();
            _httpContextMock = new Mock<IHttpContextAccessor>();

            var options = new DbContextOptionsBuilder<EfContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _efContext = new EfContext(options);

            _handler = new UpdateRecipeCommandHandler(_repoMock.Object, _mapperMock.Object, _efContext, _httpContextMock.Object);
            
            // Dodanie kategorii do kontekstu (trzeba wypełnić wymagane pole Name)
            var category = new Category
            {
                Id = 1,
                Name = "Test Category"
            };
            _efContext.Categories.Add(category);
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorized_WhenUserIsNotAuthor()
        {
            // Arrange
            var identityId = "identity-1";
            var identityUser = new IdentityUser { Id = identityId, UserName = "test" };

            var userProfile = new User { Id = 10, IdentityUserId = identityId, IdentityUser = identityUser };

            // WYPEŁNIAMY WYMAGANE POLA:
            var recipe = new Recipe
            {
                Id = 1,
                AuthorId = 99,
                Title = "Someone else's recipe",
                Description = "Desc",
                Instructions = "Instr",
                Cuisine = "Cuisine",
                ImageUrl = "Url",
                PreparationTime = 10,
                CookTime = 10,
                Servings = 2,
                CategoryId = category.Id
            };

            _efContext.Users.Add(userProfile);
            _efContext.Recipes.Add(recipe);
            await _efContext.SaveChangesAsync();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("IdentityId", identityId)
            }));
            _httpContextMock.Setup(h => h.HttpContext.User).Returns(claimsPrincipal);

            var command = new UpdateRecipeCommand { Id = 1 };

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldUpdate_WhenUserIsAuthor()
        {
            // Arrange
            var identityId = "identity-1";
            var identityUser = new IdentityUser { Id = identityId, UserName = "test" };

            var userProfile = new User { Id = 10, IdentityUserId = identityId, IdentityUser = identityUser };

            // WYPEŁNIAMY WYMAGANE POLA:
            var recipe = new Recipe
            {
                Id = 1,
                AuthorId = 10,
                Title = "My recipe",
                Description = "Desc",
                Instructions = "Instr",
                Cuisine = "Cuisine",
                ImageUrl = "Url",
                PreparationTime = 10,
                CookTime = 10,
                Servings = 2,
                CategoryId = category.Id
            };

            _efContext.Users.Add(userProfile);
            _efContext.Recipes.Add(recipe);
            await _efContext.SaveChangesAsync();
            _efContext.ChangeTracker.Clear();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("IdentityId", identityId), new Claim("email", "test@test.com") }));
            _httpContextMock.Setup(h => h.HttpContext.User).Returns(claimsPrincipal);

            var command = new UpdateRecipeCommand { Id = 1, Title = "Updated Title" };

            // Mapper też musi zwracać poprawny obiekt z wypełnionymi polami
            var mappedRecipe = new Recipe
            {
                Id = 1,
                Title = "Updated Title",
                AuthorId = 10,
                Description = "Desc",
                Instructions = "Instr",
                Cuisine = "Cuisine",
                ImageUrl = "Url",
                CategoryId = category.Id
            };

            _mapperMock.Setup(m => m.Map<Recipe>(command)).Returns(mappedRecipe);
            //_mapperMock.Setup(m => m.Map<AddUpdateRecipeDTO>(It.IsAny<Recipe>())).Returns(new AddUpdateRecipeDTO());
             _mapperMock.Setup(m => m.Map<AddUpdateRecipeDTO>(It.IsAny<Recipe>()))
                .Returns<Recipe>(r => new AddUpdateRecipeDTO
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    Instructions = r.Instructions,
                    PreparationTime = r.PreparationTime,
                    CookTime = r.CookTime,
                    Servings = r.Servings,
                    CategoryId = r.CategoryId,
                    CategoryName = category.Name, // <- poprawione, duża litera
                    Cuisine = r.Cuisine,
                    ImageUrl = r.ImageUrl,
                    AuthorName = "test@test.com",
                    RecipeIngredients = new List<AddUpdateRecipeIngredientDTO>()
                });

            _repoMock.Setup(r => r.Update(It.IsAny<Recipe>())).Returns(mappedRecipe);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repoMock.Verify(x => x.Update(It.IsAny<Recipe>()), Times.Once);
        }
    }
}