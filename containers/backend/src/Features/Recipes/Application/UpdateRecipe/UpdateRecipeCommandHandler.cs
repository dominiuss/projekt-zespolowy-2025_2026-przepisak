using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using PrzepisakApi.src.Features.Recipes.Domain;

namespace PrzepisakApi.src.Features.Recipes.Application.UpdateRecipe
{
    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, AddUpdateRecipeDTO>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;
        private readonly IEfContext _efContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateRecipeCommandHandler(
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

        public async Task<AddUpdateRecipeDTO> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var identityId = user?.FindFirst("IdentityId")?.Value;
            var authorName = user?.FindFirst("email")?.Value;

            if (identityId == null)
                throw new UnauthorizedAccessException();

            var currentUser = await _efContext.Users
                .Include(u => u.IdentityUser)
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityId, cancellationToken);

            if (currentUser == null)
                throw new UnauthorizedAccessException();

            var existingRecipe = await _efContext.Recipes
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (existingRecipe == null)
                return null;

            if (existingRecipe.AuthorId != currentUser.Id)
                throw new UnauthorizedAccessException();

            var recipeEntity = _mapper.Map<Recipe>(request);
            recipeEntity.AuthorId = currentUser.Id;

            var updatedRecipe = _recipeRepository.Update(recipeEntity);
            await _efContext.SaveChangesAsync();

            var recipeDto = _mapper.Map<AddUpdateRecipeDTO>(updatedRecipe);
            recipeDto.AuthorName = authorName;

            return recipeDto;
        }
    }
}
