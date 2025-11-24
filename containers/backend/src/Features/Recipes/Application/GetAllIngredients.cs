using MediatR;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using PrzepisakApi.src.Features.Recipes.Domain;

namespace PrzepisakApi.src.Features.Recipes.Application
{
    public class GetAllIngredients
    {
        public class Query : IRequest<List<RecipeIngredientDTO>>
        {
        }
        public class Handler : IRequestHandler<Query, List<RecipeIngredientDTO>>
        {
            private readonly IRecipeRepository _recipeRepository;

            public Handler(IRecipeRepository recipeRepository)
            {
                _recipeRepository = recipeRepository;
            }

            public async Task<List<RecipeIngredientDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _recipeRepository.GetAllIngredientsAsync();
            }
        }
    }
}
