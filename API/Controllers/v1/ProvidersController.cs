using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.DTO.PaginationDtos;
using Warehouse.API.DTO.ProviderDtos;
using Warehouse.API.Helpers.Mapping;
using Warehouse.API.OpenApi.SwaggerExamples;

namespace Warehouse.API.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ProvidersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ProvidersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("all")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<PageResponse<ProviderDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PageResponse<ProviderDto>>> GetProviders
        ([FromQuery] PaginationQueryParameters queryParams)
    {
        var (providers,totalRecords) = await _unitOfWork.Providers
            .GetAll(queryParams.PageIndex,queryParams.PageSize);

        var providerDtos = providers.Select(provider => provider.ToDto()).ToList();

        var paginationInfo = new PaginationInfo(queryParams.PageIndex, queryParams.PageSize, totalRecords);

        var pageResponse = new PageResponse<ProviderDto>(providerDtos, paginationInfo);
            
        return Ok(pageResponse);
    }

    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ProviderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProviderDto>> GetProvider([FromRoute] Guid id)
    {
        var providerEntity = await _unitOfWork.Providers.Get(id);
        
        if (providerEntity == null)
            return GetProviderNotFoundResponse(id);

        return Ok(providerEntity.ToDto());
    }

    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerRequestExample(typeof(ProviderUpdateDto),typeof(ProviderUpdateDtoExample))]
    public async Task<ActionResult<ProviderDto>> CreateProvider(
        [FromBody] Provider? provider)
    {
        await _unitOfWork.Providers.Add(provider!);
        await _unitOfWork.Complete();

        return CreatedAtAction(nameof(GetProvider),
            new { id = provider!.Id }, provider.ToDto());
    }

    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(ProviderUpdateDto),typeof(ProviderUpdateDtoExample))]
    public async Task<IActionResult> UpdateProvider(
        [FromBody]Provider? provider, [FromRoute] Guid id)
    {
        var providerUpdateSuccessfully = await _unitOfWork.Providers.Update(provider!);

        if (!providerUpdateSuccessfully)
            return GetProviderNotFoundResponse(id);

        await _unitOfWork.Complete();
        
        return NoContent();

    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProvider(Guid id)
    {
        var wasProviderRemoved = await _unitOfWork.Providers.Remove(id);

        if (!wasProviderRemoved)
            return GetProviderNotFoundResponse(id);

        await _unitOfWork.Complete();
        return NoContent();
    }
    
    private NotFoundObjectResult GetProviderNotFoundResponse(Guid id)
    {
        return NotFound($"Provider with id {id} does not exist");
    }
}