using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Warehouse.API.Common.Mapping;
using Warehouse.API.DTO.Brand;

namespace Warehouse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public BrandsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<BrandDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrands()
    {
        var brands = await _unitOfWork.Brands.GetAll();

        var brandDtos = brands.Select(product => product.ToDto()).ToList();

        return Ok(brandDtos);
    }

    [HttpGet("{id:guid}")]
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
    [ProducesResponseType(typeof(BrandDto), 201)]
    public async Task<ActionResult<BrandDto>> CreateBrand(BrandUpdateDto brandDto)
    {
        var brand = brandDto.ToEntity();

        await _unitOfWork.Brands.Add(brand);
        await _unitOfWork.Complete();

        return CreatedAtAction(nameof(GetBrand),
            new { id = brand.Id }, brand.ToDto());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBrand(BrandUpdateDto brandDto, Guid id)
    {
        var brandEntity = brandDto.ToEntity(id);
        var brandUpdatedSuccessfully = await _unitOfWork.Brands.Update(brandEntity);

        if (!brandUpdatedSuccessfully)
            return GetBrandNotFoundResponse(id);

        await _unitOfWork.Complete();

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

        return NoContent();
    }

    private NotFoundObjectResult GetBrandNotFoundResponse(Guid id)
    {
        return NotFound($"Brand with id {id} does not exist {id}");
    }
}