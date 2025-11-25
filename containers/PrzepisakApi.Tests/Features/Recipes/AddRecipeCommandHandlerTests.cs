using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Recipes.Application.AddRecipe;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using PrzepisakApi.src.Features.Recipes.Domain;
using PrzepisakApi.src.Features.UserProfile.Domain;
using System.Security.Claims;

namespace PrzepisakApi.Tests.Features.Recipes
{
    public class AddRecipeCommandHandlerTests
    {
        private readonly Mock<IRecipeRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IHttpContextAccessor> _httpContextMock;
        private readonly EfContext _efContext;
        private readonly AddRecipeCommandHandler _handler;

        public AddRecipeCommandHandlerTests()
        {
            _repoMock = new Mock<IRecipeRepository>();
            _mapperMock = new Mock<IMapper>();
            _httpContextMock = new Mock<IHttpContextAccessor>();

            var options = new DbContextOptionsBuilder<EfContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _efContext = new EfContext(options);

            _handler = new AddRecipeCommandHandler(_repoMock.Object, _mapperMock.Object, _efContext, _httpContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldAddRecipe_WhenUserIsAuthorized()
        {
            // Arrange
            var command = new AddRecipeCommand { Title = "New Recipe" };
            var identityId = "identity-1";

            // 1. Tworzymy IdentityUser
            var identityUser = new IdentityUser { Id = identityId, UserName = "testUser", Email = "test@test.com" };

            // POPRAWKA: Używamy Set<IdentityUser>(), aby dodać do tabeli Identity, a nie do tabeli Twoich Userów
            _efContext.Set<IdentityUser>().Add(identityUser);

            // 2. Tworzymy UserProfile powiązany z IdentityUser
            var userProfile = new User { Id = 10, IdentityUserId = identityId, IdentityUser = identityUser };

            _efContext.Users.Add(userProfile); // Tutaj dodajemy do tabeli domenowej (Twojej klasy User)
            await _efContext.SaveChangesAsync();

            // Mockujemy HttpContext
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("IdentityId", identityId),
                new Claim("email", "test@test.com")
            }));
            _httpContextMock.Setup(h => h.HttpContext.User).Returns(claimsPrincipal);

            // Test naprawy kategorii
            var category = new Category { Id = 1, Name = "Test Category" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // 3. Mockujemy mapper - obiekt Recipe MUSI mieć wypełnione pola wymagane!
            var recipe = new Recipe
            {
                Title = "New Recipe",
                AuthorId = 10,
                Description = "Test Description",
                Instructions = "Test Instructions",
                Cuisine = "Polish",
                ImageUrl = "http://image.url",
                PreparationTime = 30,
                CookTime = 45,
                CategoryId = category.Id,
                Servings = 4
            };

            _mapperMock.Setup(m => m.Map<Recipe>(command)).Returns(recipe);
            _mapperMock.Setup(m => m.Map<AddUpdateRecipeDTO>(It.IsAny<Recipe>())).Returns(new AddUpdateRecipeDTO());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repoMock.Verify(x => x.Add(It.Is<Recipe>(r => r.AuthorId == 10)), Times.Once);
            recipe.AuthorId.Should().Be(10);
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorized_WhenUserNotFoundInContext()
        {
            // Arrange
            _httpContextMock.Setup(h => h.HttpContext.User).Returns((ClaimsPrincipal)null);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(new AddRecipeCommand(), CancellationToken.None));
        }
    }
}