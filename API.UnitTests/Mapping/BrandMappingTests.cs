using Domain.Entities;
using Domain.UnitTests.Fixtures;
using Domain.ValueObjects;
using Warehouse.API.Common.Mapping;
using Warehouse.API.DTO;

namespace API.UnitTests.Mapping;

public class BrandMappingTests
{
    private readonly Brand _brand;
    private readonly BrandDto _brandDto;


    public BrandMappingTests()
    {
        _brand = BrandsFixture.GetTestBrand();
        
        _brandDto = new BrandDto
        {
            BrandId = Guid.NewGuid(),
            Name = _brand.Name.Value,
            Image = _brand.Image.Value,
            Description = _brand.Description.Value
        }; 
    }
    
    [Fact]
    public void Should_MapBrandToBrandDto_When_BrandIsValid()
    {
        var mappedBrandDto = _brand.ToDto();
        
        Assert.Equal(_brand.Id, mappedBrandDto.BrandId);
        Assert.Equal(_brand.Name.Value, mappedBrandDto.Name);
        Assert.Equal(_brand.Image.Value, mappedBrandDto.Image);
        Assert.Equal(_brand.Description.Value, mappedBrandDto.Description);
    }
    
    [Fact]
    public void Should_MapBrandDtoToBrand_When_BrandDtoIsValid()
    {
        var mappedBrand = _brandDto.ToEntity();
        
        Assert.Equal(_brandDto.BrandId, mappedBrand.Id);
        Assert.Equal(_brandDto.Name, mappedBrand.Name.Value);
        Assert.Equal(_brandDto.Image, mappedBrand.Image.Value);
        Assert.Equal(_brandDto.Description, mappedBrand.Description.Value);
    }
}
