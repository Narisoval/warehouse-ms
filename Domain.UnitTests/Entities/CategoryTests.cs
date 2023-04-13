using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities;

public class CategoryTests
{
    
    private readonly CategoryName _categoryName = CategoryName.From("type").Value;
    
    [Fact]
    public void Should_CreateCategory_When_CreatingCategoryWithId()
    {
        //Arrange
        var id = Guid.NewGuid();
        
        //Act 
        var sut = Category.Create(id, _categoryName);
        
        //Assert
        sut.IsSuccess.Should().BeTrue();
        sut.Value.Id.Should().Be(id);
        sut.Value.Name.Should().BeEquivalentTo(_categoryName);

    }
    
    [Fact]
    public void Should_CreateCategory_When_CreatingCategoryWithoutId()
    {
        //Act 
        var sut = Category.Create(_categoryName);
        
        //Assert
        sut.IsSuccess.Should().BeTrue();
        sut.Value.Id.Should().NotBe(Guid.Empty);
        sut.Value.Name.Should().BeEquivalentTo(_categoryName);
    }
    
    [Fact]
    public void Should_ReturnFailedResult_When_CategoryIsCreatedWithEmptyGuid()
    {
        //Arrange 
        var id = Guid.Empty;
        
        //Act
        var sut = Category.Create(id, _categoryName);
        
        //Assert
        sut.AssertIsFailed(1);
    }
    
}