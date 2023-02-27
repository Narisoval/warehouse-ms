using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.Common.Mapping;
using Warehouse.API.DTO.BrandDtos;
using Warehouse.API.DTO.SwaggerExamples;

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
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<BrandDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrands()
    {
        var brands = await _unitOfWork.Brands.GetAll();

        var brandDtos = brands.Select(product => product.ToDto()).ToList();

        return Ok(brandDtos);
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
    public async Task<ActionResult<BrandDto>> CreateBrand([FromBody] Brand brand)
    {
        await _unitOfWork.Brands.Add(brand);
        await _unitOfWork.Complete();

        return CreatedAtAction(nameof(GetBrand),
            new { id = brand.Id }, brand.ToDto());
    }

    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(BrandUpdateDto),typeof(BrandUpdateDtoExample))]
    public async Task<IActionResult> UpdateBrand(
        [FromBody] Brand brand
        ,[FromRoute] Guid id)
    {
        var brandUpdatedSuccessfully = await _unitOfWork.Brands.Update(brand);

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
        return NotFound($"Brand with id {id} does not exist");
    }
}