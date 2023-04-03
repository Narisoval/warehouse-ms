using Domain.Entities;
using Warehouse.API.DTO.CategoryDtos;
using Warehouse.API.Messaging.Events.CategoryEvents;

namespace Warehouse.API.Helpers.Mapping;

public static class CategoryMapping 
{
    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto
        {
            CategoryId = category.Id,
            Name = category.Name.Value,
            ParentId = category.Id,
            SubCategories = category.SubCategories?.Select(x => x.ToDto()).ToList()
        };
    }

    public static CategoryCreatedEvent ToCreatedEvent(this Category category)
    {
        return new CategoryCreatedEvent
        {
            Id = category.Id,
            Name = category.Name.Value,
        };
    }
    
    public static CategoryUpdatedEvent ToUpdatedEvent(this Category category)
    {
        return new CategoryUpdatedEvent
        {
            Id = category.Id,
            Name = category.Name.Value,
        };
    }
}