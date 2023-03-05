using Domain.Entities;
using Warehouse.API.DTO.CategoryDtos;

namespace Warehouse.API.Helpers.Mapping;

public static class CategoryMapping 
{
    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto
        {
            CategoryId = category.Id,
            Name = category.Name.Value,
        };
    }
}