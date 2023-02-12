using Domain.Entities;
using Domain.ValueObjects;
using Warehouse.API.DTO;

namespace Warehouse.API.Common.Mapping;

public static class BrandMappingConfig 
{
    public static BrandDto ToDto(this Brand brand)
    {
        return new BrandDto
        {
            BrandId = brand.Id,
            Name = brand.Name.Value,
            Image = brand.Image.Value,
            Description = brand.Description.Value
        };
        
    }
    
    public static Brand ToEntity(this BrandDto brandDto)
    {
        return Brand.Create(
            brandDto.BrandId,
            BrandName.From(brandDto.Name),
            Image.From(brandDto.Image),
            BrandDescription.From(brandDto.Description));
    }
}
