using Domain.Entities;
using Warehouse.API.DTO.BrandDtos;

namespace Warehouse.API.Helpers.Mapping;

public static class BrandMapping 
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
}
