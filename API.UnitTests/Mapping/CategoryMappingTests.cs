using Domain.Entities;
using Domain.UnitTests.Fixtures;
using Domain.ValueObjects;
using Warehouse.API.Common.Mapping;
using Warehouse.API.DTO;

namespace API.UnitTests.Mapping;

public class CategoryMappingTests
{
    private readonly Category _category;
    private readonly CategoryDto _categoryDto;

    public CategoryMappingTests()
    {
        _category = CategoriesFixture.GetTestCategory(); 
        _categoryDto = new CategoryDto
        {
            CategoryId = Guid.NewGuid(),
            Name = "Test DTO Category"
        };
    }

    [Fact]
    public void Should_MapCategoryToCategoryDto_When_CategoryIsValid()
    {
        var mappedCategoryDto = _category.ToDto();
    
        Assert.Equal(_category.Id, mappedCategoryDto.CategoryId);
        Assert.Equal(_category.Name.Value, mappedCategoryDto.Name);
    }

    [Fact]
    public void Should_MapCategoryDtoToCategory_When_CategoryDtoIsValid()
    {
        var mappedCategory = _categoryDto.ToEntity();
    
        Assert.Equal(_categoryDto.CategoryId, mappedCategory.Id);
        Assert.Equal(_categoryDto.Name, mappedCategory.Name.Value);
    }
}
 