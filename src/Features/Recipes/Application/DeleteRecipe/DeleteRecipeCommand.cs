using MediatR;

namespace PrzepisakApi.src.Features.Recipes.Application.DeleteRecipe
{
    public record DeleteRecipeCommand(int Id) : IRequest<Unit>
    {
    }
}
