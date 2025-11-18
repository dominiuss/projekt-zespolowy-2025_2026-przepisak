using MapsterMapper;
using MediatR;
using PrzepisakApi.src.Features.Recipes.Domain;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using PrzepisakApi.src.Database;

namespace PrzepisakApi.src.Features.Recipes.Application.AddRecipe
{
    public class AddRecipeCommandHandler : IRequestHandler<AddRecipeCommand, AddUpdateRecipeDTO>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IEfContext _efContext;
        private readonly IMapper _mapper;
        public AddRecipeCommandHandler(IRecipeRepository recipeRepository,IMapper mapper, IEfContext efContext) 
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
            _efContext = efContext;
        }
        public async Task<AddUpdateRecipeDTO> Handle(AddRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = _mapper.Map<Recipe>(request);
            _recipeRepository.Add(recipe);
            await _efContext.SaveChangesAsync();
            var recipeDto = _mapper.Map<AddUpdateRecipeDTO>(recipe);
            return recipeDto;
        }
    }
}
