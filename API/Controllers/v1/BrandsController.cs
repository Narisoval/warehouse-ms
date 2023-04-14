using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.MessageBroker.EventBus;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.DTO.BrandDtos;
using Warehouse.API.DTO.PaginationDtos;
using Warehouse.API.Helpers.Mapping;
using Warehouse.API.Messaging.Events.BrandEvents;
using Warehouse.API.OpenApi.SwaggerExamples;

namespace Warehouse.API.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class BrandsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IEventBus _eventBus; 
    
    public BrandsController(IUnitOfWork unitOfWork, IEventBus eventBus)
    {
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    [HttpGet("all")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(PageResponse<BrandDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PageResponse<BrandDto>>> GetBrands(
        [FromQuery] PaginationQueryParameters queryParams)
    {
        var (brands, totalRecords) = await _unitOfWork.Brands
            .GetAll(queryParams.PageIndex,queryParams.PageSize);
        
        var brandDtos = brands.Select(product => product.ToDto()).ToList();

        var paginationInfo = new PaginationInfo(queryParams.PageIndex, queryParams.PageSize, totalRecords);

        var pageResponse = new PageResponse<BrandDto>(brandDtos,paginationInfo);
        
        return Ok(pageResponse);
    }

    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(BrandDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BrandDto>> GetBrand([FromRoute] Guid id)
    {
        var brand = await _unitOfWork.Brands.Get(id);
        if (brand == null)
            return GetBrandNotFoundResponse(id);

        return Ok(brand.ToDto());
    }

    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(BrandDto), 201)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(BrandUpdateDto),typeof(BrandUpdateDtoExample))]
    public async Task<ActionResult<BrandDto>> CreateBrand([FromBody] Brand? brand)
    {
        await _unitOfWork.Brands.Add(brand!);
        
        await _unitOfWork.Complete();

        await _eventBus.PublishAsync(brand!.ToCreatedEvent());
        
        return CreatedAtAction(nameof(GetBrand),
            new { id = brand!.Id }, brand.ToDto());
    }

    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(BrandUpdateDto),typeof(BrandUpdateDtoExample))]
    public async Task<IActionResult> UpdateBrand(
        [FromBody] Brand? brand
        ,[FromRoute] Guid id)
    {
        var brandUpdatedSuccessfully = await _unitOfWork.Brands.Update(brand!);

        if (!brandUpdatedSuccessfully)
            return GetBrandNotFoundResponse(id);

        await _unitOfWork.Complete();

        await _eventBus.PublishAsync(brand!.ToUpdatedEvent());
        
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBrand(Guid id)
    {
        var wasBrandRemoved = await _unitOfWork.Brands.Remove(id);

        if (!wasBrandRemoved)
            return GetBrandNotFoundResponse(id);

        await _unitOfWork.Complete();

        await _eventBus.PublishAsync(new BrandDeletedEvent{ Id = id});
        
        return NoContent();
    }

    private NotFoundObjectResult GetBrandNotFoundResponse(Guid id)
    {
        return NotFound($"Brand with id {id} does not exist");
    }
}