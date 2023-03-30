using Domain.Entities;
using FluentResults;
using Infrastructure.Interfaces;
using Infrastructure.MessageBroker.EventBus;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.DTO.ProductDtos;
using Warehouse.API.DTO.SwaggerExamples;
using Warehouse.API.Helpers.Extensions;
using Warehouse.API.Helpers.Mapping;
using Warehouse.API.Messaging.Events.ProductEvents;

namespace Warehouse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public ProductsController(IUnitOfWork unitOfWork, IEventBus eventBus)
    {
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>),StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(
        [FromQuery] int pageIndex = 1, 
        [FromQuery] int pageSize = 15)
    {
        var products = await _unitOfWork.Products.GetAll(pageIndex,pageSize);
        
        var productDtos = products.Select(product => product.ToDto()).ToList();
        
        return Ok(productDtos);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto),200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProduct([FromRoute] Guid id)
    {
        var productEntity = await _unitOfWork.Products.Get(id);
        
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
        var productAddedResult = await _unitOfWork.Products.Add(product!);

        if (productAddedResult.IsFailed)
            return HandleRepositoryErrors(productAddedResult.Errors);
        
        await _unitOfWork.Complete();

        await _eventBus.PublishAsync(product!.ToCreatedEvent());
            
        return CreatedAtAction(nameof(GetProduct),
            new { id = product!.Id }, product.ToDto());
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
           return HandleRepositoryErrors(productUpdatedSuccessfully.Errors);
        
        if (!productUpdatedSuccessfully.Value)
            return GetProductNotFoundResponse(id);

        await _unitOfWork.Complete();

        await _eventBus.PublishAsync(product!.ToUpdatedEvent());
        
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

        await _eventBus.PublishAsync(new ProductDeletedEvent(id));
        
        return NoContent();
    }

    private NotFoundObjectResult GetProductNotFoundResponse(Guid id)
    {
        return NotFound($"Product with id {id} does not exist");
    }

    private ActionResult HandleRepositoryErrors(List<IError> errors)
    {
        errors.AddModelErrors(ModelState,"product");
        return BadRequest(ModelState);
    }
}