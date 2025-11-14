using MapsterMapper;
using MediatR;
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
        public UpdateRecipeCommandHandler(IRecipeRepository recipeRepository, IMapper mapper, IEfContext efContext)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
            _efContext = efContext;
        }
        public async Task<AddUpdateRecipeDTO> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipeEntity = _mapper.Map<Recipe>(request);
            var updatedRecipe = _recipeRepository.Update(recipeEntity);
            await _efContext.SaveChangesAsync();
            var recipeDto = _mapper.Map<AddUpdateRecipeDTO>(recipeEntity);
            return recipeDto;
        }
    }
}
