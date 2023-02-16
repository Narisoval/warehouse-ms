using Domain.Entities;
using Domain.ValueObjects;
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

    public static Category ToEntity(this CategoryDto categoryDto)
    {
        return Category.Create(
            categoryDto.CategoryId,
            CategoryName.From(categoryDto.Name));
    }
    
    public static Category ToEntity(this CategoryUpdateDto categoryDto)
    {
        return Category.Create(
            CategoryName.From(categoryDto.Name));
    }

    public static Category ToEntity(this CategoryUpdateDto categorydto, Guid id)
    {
        return Category.Create(
            id,
            CategoryName.From(categorydto.Name));
    }
}