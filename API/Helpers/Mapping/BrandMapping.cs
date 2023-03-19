using Domain.Entities;
using Warehouse.API.DTO.BrandDtos;
using Warehouse.API.Messaging.Events.BrandEvents;

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

    public static BrandUpdatedEvent ToUpdatedEvent(this Brand brand)
    {
        return new BrandUpdatedEvent
        {
            Id = brand.Id,
            Name = brand.Name.Value,
            Image = brand.Image.Value,
            Description = brand.Description.Value
        };
    }

    public static BrandCreatedEvent ToCreatedEvent(this Brand brand)
    {
        return new BrandCreatedEvent
        {
            Id = brand.Id,
            Name = brand.Name.Value,
            Image = brand.Image.Value,
            Description = brand.Description.Value
        };
    }
}