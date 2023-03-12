using Domain.Entities;
using FluentResults;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.DTO.ProductDtos;
using Warehouse.API.DTO.SwaggerExamples;
using Warehouse.API.Helpers.Extensions;
using Warehouse.API.Helpers.Mapping;

namespace Warehouse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>),StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        var products = await _unitOfWork.Products.GetAllProductsWithProvider();
        
        var productDtos = products.Select(product => product.ToDto()).ToList();
        
        return Ok(productDtos);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto),200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProduct([FromRoute] Guid id)
    {
        var productEntity = await _unitOfWork.Products.GetProductWithProvider(id);
        
        if (productEntity == null)
            return GetProductNotFoundResponse(id);

        return Ok(productEntity.ToDto());
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), 201)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(ProductUpdateDto),typeof(ProductUpdateDtoExample))]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] Product? product)
    {
        var productResult = await _unitOfWork.Products.Add(product!);

        if (productResult.IsFailed)
            return HandleForeignKeyViolations(productResult.Errors);
        
        await _unitOfWork.Complete();

        return CreatedAtAction(nameof(GetProduct), 
            new { id = product.Id}, product.ToDto());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerRequestExample(typeof(ProductUpdateDto),typeof(ProductUpdateDtoExample))]
    public async Task<IActionResult> UpdateProduct([FromBody] Product? product, [FromRoute] Guid id)
    {
        var productUpdatedSuccessfully = await _unitOfWork.Products.Update(product!);

        if (productUpdatedSuccessfully.IsFailed)
           return HandleForeignKeyViolations(productUpdatedSuccessfully.Errors);
        
        if (!productUpdatedSuccessfully.Value)
            return GetProductNotFoundResponse(id);

        await _unitOfWork.Complete();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
    {
        var wasProductRemoved = await _unitOfWork.Products.Remove(id);

        if (!wasProductRemoved)
            GetProductNotFoundResponse(id);

        await _unitOfWork.Complete();

        return NoContent();
    }

    private NotFoundObjectResult GetProductNotFoundResponse(Guid id)
    {
        return NotFound($"Product with id {id} does not exist");
    }

    private ActionResult HandleForeignKeyViolations(List<IError> errors)
    {
        errors.AddModelErrors(ModelState,"product");
        return BadRequest(ModelState);
    }
    
}