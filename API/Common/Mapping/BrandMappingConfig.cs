using Domain.Entities;
using Domain.ValueObjects;
using Warehouse.API.DTO;
using Warehouse.API.DTO.Brand;

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
    
    public static BrandUpdateDto ToUpdateDto(this Brand brand)
    {
        return new BrandUpdateDto
        {
            Name = brand.Name.Value,
            Description = brand.Description.Value,
            Image = brand.Image.Value
        };
    }
    
    public static Brand ToEntity(this BrandUpdateDto updateDto)
    {
        return Brand.Create(
            BrandName.From(updateDto.Name),
            Image.From(updateDto.Image),
            BrandDescription.From(updateDto.Description));
    }

    public static Brand ToEntity(this BrandUpdateDto dto, Guid id)
    {
        return Brand.Create(
            id,
            BrandName.From(dto.Name),
            Image.From(dto.Image),
            BrandDescription.From(dto.Description));
    }
}
