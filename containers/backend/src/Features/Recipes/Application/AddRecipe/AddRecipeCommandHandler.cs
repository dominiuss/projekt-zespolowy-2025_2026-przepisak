using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using PrzepisakApi.src.Features.Recipes.Domain;

namespace PrzepisakApi.src.Features.Recipes.Application.AddRecipe
{
    public class AddRecipeCommandHandler : IRequestHandler<AddRecipeCommand, AddUpdateRecipeDTO>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IEfContext _efContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddRecipeCommandHandler(
            IRecipeRepository recipeRepository,
            IMapper mapper,
            IEfContext efContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
            _efContext = efContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AddUpdateRecipeDTO> Handle(AddRecipeCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var identityId = user?.FindFirst("IdentityId")?.Value;
            var authorName = user?.FindFirst("email")?.Value;

            if (identityId == null)
                throw new UnauthorizedAccessException();

                // --- SPRAWDZENIE LUB UTWORZENIE KATEGORII ---
            var category = await _efContext.Categories
                .FirstOrDefaultAsync(c => c.Name == request.CategoryName, cancellationToken);

            if (category == null)
            {
                category = new Category { Name = request.CategoryName };
                _efContext.Categories.Add(category);
                await _efContext.SaveChangesAsync(cancellationToken);
            }
            
            var currentUser = await _efContext.Users
                .Include(u => u.IdentityUser)
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityId, cancellationToken);

            if (currentUser == null)
                throw new UnauthorizedAccessException();

            var recipe = _mapper.Map<Recipe>(request);
            recipe.AuthorId = currentUser.Id;
            recipe.CategoryId = category.Id;   // przypisujemy ID kategorii
            recipe.CreatedAt = DateTime.UtcNow;
            recipe.UpdatedAt = DateTime.UtcNow;

            _recipeRepository.Add(recipe);
            await _efContext.SaveChangesAsync();

            var recipeDto = _mapper.Map<AddUpdateRecipeDTO>(recipe);
            recipeDto.AuthorName = authorName;

            return recipeDto;
        }
    }
}
