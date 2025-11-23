using MediatR;
using Microsoft.AspNetCore.Mvc;
using PrzepisakApi.src.Features.Recipes.Application.DTOs;
using PrzepisakApi.src.Features.Recipes.Application.ViewAllRecipes;
using PrzepisakApi.src.Features.Recipes.Application.AddRecipe;
using MapsterMapper;
using PrzepisakApi.src.Features.Recipes.Application.DeleteRecipe;
using PrzepisakApi.src.Features.Recipes.Application.ViewRecipe;
using PrzepisakApi.src.Features.Recipes.Application.UpdateRecipe;
using PrzepisakApi.src.Features.Recipes.Application.Search.SearchByRecipeTitle;
using PrzepisakApi.src.Features.Recipes.Application.Search.SearchByAuthorName;
using PrzepisakApi.src.Features.Recipes.Application;
using Microsoft.AspNetCore.Authorization;

namespace PrzepisakApi.src.Features.Recipes.Api
{

    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RecipesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<RecipeOverviewDTO>>> GetAllRecipes([FromQuery] ViewAllRecipesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeDTO>> GetRecipeById([FromRoute] int id)
        {
            var query = new ViewRecipeQuery { Id = id };
            var result = await _mediator.Send(query);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AddUpdateRecipeDTO>> Add([FromBody] AddUpdateRecipeDTO addUpdateRecipeDTO)
        {
            var command = _mapper.Map<AddRecipeCommand>(addUpdateRecipeDTO);
            var result = await _mediator.Send(command);
            return result != null ? CreatedAtAction(nameof(GetRecipeById), new { id = result.Id }, result) : BadRequest();
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<AddUpdateRecipeDTO>> Update([FromRoute] int id, [FromBody] AddUpdateRecipeDTO addUpdateRecipeDTO)
        {
            var command = _mapper.Map<UpdateRecipeCommand>(addUpdateRecipeDTO);
            var result = await _mediator.Send(command);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var response = await _mediator.Send(new DeleteRecipeCommand(id));
            return NoContent();
        }
        [AllowAnonymous]
        [HttpGet("search/title")]
        public async Task<ActionResult<List<RecipeOverviewDTO>>> SearchByRecipeTitle([FromQuery] string title)
        {
            var query = new SearchByRecipeTitleQuery { Title = title };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("search/name")]
        public async Task<ActionResult<List<RecipeOverviewDTO>>> SearchByAuthorName([FromQuery] string name)
        {
            var query = new SearchByAuthorNameQuery { AuthorName = name };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("ingredient")]
        public async Task<ActionResult<List<RecipeIngredientDTO>>> GetAllIngredients()
        {
            var result = await _mediator.Send(new GetAllIngredients.Query());
            return Ok(result);
        }
    }
}