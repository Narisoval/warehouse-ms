using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Warehouse.API.Common.Mapping;
using Warehouse.API.DTO.Category;

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
    public async Task<ActionResult<CategoryDto>> CreateCategory(CategoryUpdateDto categoryDto)
    {
        var categoryEntity = categoryDto.ToEntity();
        
        await _unitOfWork.Categories.Add(categoryEntity);
        await _unitOfWork.Complete();

        return CreatedAtAction(nameof(GetCategory),
            new { id = categoryEntity.Id }, categoryEntity.ToDto());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory(CategoryUpdateDto categoryDto, Guid id)
    {
        var categoryEntity = categoryDto.ToEntity(id);
        var categoryUpdatedSuccessfully = await _unitOfWork.Categories.Update(categoryEntity);

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