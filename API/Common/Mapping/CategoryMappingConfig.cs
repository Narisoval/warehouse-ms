using Domain.Entities;
using Warehouse.API.DTO.Category;

namespace Warehouse.API.Common.Mapping;

public static class CategoryMappingConfig 
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