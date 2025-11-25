using MediatR;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Recipes.Domain;

namespace PrzepisakApi.src.Features.Recipes.Application.DeleteRecipe
{
    public class DeleteRecipeCommandHandler : IRequestHandler<DeleteRecipeCommand, Unit>
    {
        private readonly IRecipeRepository _repository;
        private readonly IEfContext _efContext;
        public DeleteRecipeCommandHandler(IRecipeRepository recipeRepository, IEfContext efContext )
        {
            _repository = recipeRepository;
            _efContext = efContext;
        }
        public async Task<Unit> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
        {
            _repository.Delete(request.Id);
            await _efContext.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
