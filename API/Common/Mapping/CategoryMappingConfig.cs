using Domain.Entities;
using Domain.ValueObjects;
using Warehouse.API.DTO;

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

    public static Category ToEntity(this CategoryDto categoryDto)
    {
        return Category.Create(
            categoryDto.CategoryId,
            CategoryName.From(categoryDto.Name));
    }
}