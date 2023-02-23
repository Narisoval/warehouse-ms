using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Warehouse.API.Common.Mapping;
using Warehouse.API.DTO;
using Warehouse.API.DTO.Category;
using Warehouse.API.DTO.Product;

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
    [ProducesResponseType(typeof(CategoryDto), 201)]
    public async Task<ActionResult<ProductDto>> CreateProduct(ProductUpdateDto productDto)
    {
        var productEntity = productDto.ToEntity();
       
        await _unitOfWork.Products.Add(productEntity);
        await _unitOfWork.Complete();

        return CreatedAtAction(nameof(GetProduct), 
            new { id = productEntity.Id}, productEntity.ToDto());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(ProductUpdateDto productDto, Guid id)
    {
        var productEntity = productDto.ToEntity(id);
        var productUpdateSuccessfully = await _unitOfWork.Products.Update(productEntity);

        if (!productUpdateSuccessfully)
            return GetProductNotFoundResponse(id);

        await _unitOfWork.Complete();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(Guid id)
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
}