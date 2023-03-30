using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.DTO.ProviderDtos;
using Warehouse.API.DTO.SwaggerExamples;
using Warehouse.API.Helpers.Mapping;

namespace Warehouse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProvidersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ProvidersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("all")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<ProviderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProviderDto>>> GetProviders(
        [FromQuery] int pageIndex = 1, 
        [FromQuery] int pageSize = 15)
    {
        var providers = await _unitOfWork.Providers.GetAll();

        var providerDtos = providers.Select(provider => provider.ToDto()).ToList();
        return Ok(providerDtos);

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