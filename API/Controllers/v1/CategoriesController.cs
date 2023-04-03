using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.MessageBroker.EventBus;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.DTO.CategoryDtos;
using Warehouse.API.DTO.PaginationDtos;
using Warehouse.API.Helpers.Mapping;
using Warehouse.API.Messaging.Events.CategoryEvents;
using Warehouse.API.OpenApi.SwaggerExamples;

namespace Warehouse.API.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class CategoriesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    
    public CategoriesController(IUnitOfWork unitOfWork, IEventBus eventBus)
    {
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(PageResponse<CategoryDto>),StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = await _unitOfWork.Categories.GetAll();

        var categoryDtos = categories.Select(category => category.ToDto()).ToList();

        return Ok(categoryDtos);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CategoryDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDto>> GetCategory([FromRoute] Guid id)
    {
        var categoryEntity = await _unitOfWork.Categories.Get(id);
        
        if (categoryEntity == null)
            return GetCategoryNotFoundResponse(id);
        
        return Ok(categoryEntity.ToDto());
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryDto), 201)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(CategoryUpdateDto),typeof(CategoryUpdateDtoExample))]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] Category? category)
    {
        await _unitOfWork.Categories.Add(category!);
        
        await _unitOfWork.Complete();

        await _eventBus.PublishAsync(category!.ToCreatedEvent());

        return CreatedAtAction(nameof(GetCategory),
            new { id = category!.Id }, category.ToDto());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerRequestExample(typeof(CategoryUpdateDto),typeof(CategoryUpdateDtoExample))]
    public async Task<IActionResult> UpdateCategory([FromBody] Category? category, [FromRoute] Guid id)
    {
        var categoryUpdatedSuccessfully = await _unitOfWork.Categories.Update(category!);

        if (!categoryUpdatedSuccessfully)
            return GetCategoryNotFoundResponse(id);
        
        await _unitOfWork.Complete();

        await _eventBus.PublishAsync(category!.ToUpdatedEvent());
        
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var wasBrandRemoved = await _unitOfWork.Categories.Remove(id);
        
        if (!wasBrandRemoved)
            return GetCategoryNotFoundResponse(id);
        
        await _unitOfWork.Complete();

        await _eventBus.PublishAsync(new CategoryDeletedEvent(id));
        
        return NoContent();
    }

    private NotFoundObjectResult GetCategoryNotFoundResponse(Guid id)
    {
        return NotFound($"Category with id {id} does not exist");
    }
}