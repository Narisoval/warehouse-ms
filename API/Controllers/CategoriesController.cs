using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.Common.Mapping;
using Warehouse.API.DTO.CategoryDtos;
using Warehouse.API.DTO.SwaggerExamples;

namespace Warehouse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoriesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>),StatusCodes.Status200OK)]
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
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] Category category)
    {
        await _unitOfWork.Categories.Add(category);
        await _unitOfWork.Complete();

        return CreatedAtAction(nameof(GetCategory),
            new { id = category.Id }, category.ToDto());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerRequestExample(typeof(CategoryUpdateDto),typeof(CategoryUpdateDtoExample))]
    public async Task<IActionResult> UpdateCategory([FromBody] Category category, [FromRoute] Guid id)
    {
        var categoryUpdatedSuccessfully = await _unitOfWork.Categories.Update(category);

        if (!categoryUpdatedSuccessfully)
            return GetCategoryNotFoundResponse(id);
        
        await _unitOfWork.Complete();

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
        return NoContent();
    }

    private NotFoundObjectResult GetCategoryNotFoundResponse(Guid id)
    {
        return NotFound($"Category with id {id} does not exist");
    }
}