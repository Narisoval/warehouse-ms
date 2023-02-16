using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Warehouse.API.Common.Mapping;
using Warehouse.API.DTO.Provider;

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
    [ProducesResponseType(typeof(IEnumerable<ProviderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProviderDto>>> GetProviders()
    {
        var providers = await _unitOfWork.Providers.GetAll();

        var providerDtos = providers.Select(provider => provider.ToDto()).ToList();
        return Ok(providerDtos);

    }

    [HttpGet("{id:guid}")]
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
    [ProducesResponseType(typeof(ProviderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProviderDto>> CreateProvider(ProviderUpdateDto providerDto)
    {
        var providerEntity = providerDto.ToEntity();

        await _unitOfWork.Providers.Add(providerEntity);
        await _unitOfWork.Complete();

        return CreatedAtAction(nameof(GetProvider),
            new { id = providerEntity.Id }, providerEntity.ToDto());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProvider(ProviderUpdateDto providerDto, Guid id)
    {
        var providerEntity = providerDto.ToEntity(id);
        var providerUpdateSuccessfully = await _unitOfWork.Providers.Update(providerEntity);

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