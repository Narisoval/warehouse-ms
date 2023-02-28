using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities;

public class CategoryTests
{
    [Fact]
    public void Should_ReturnFailedResult_When_ArgumentsAreNull()
    {
        //Arrange
        CategoryName? categoryName = null;
        var id = Guid.NewGuid();
        
        //Act
        var categoryResult = Category.Create(id, categoryName);
        
        //Assert
        categoryResult.IsFailed.Should().BeTrue();
    }
    [Fact]
    public void Should_CreateCategory_When_ArgumentsAreValid()
    {
        //Arrange
        CategoryName? categoryName = CategoryName.From("type").Value;
        var id = Guid.NewGuid();
        
        //Act 
        var sut = Category.Create(id, categoryName);
        
        //Assert
        sut.IsSuccess.Should().BeTrue();
        sut.Value.Name.Should().BeEquivalentTo(categoryName);

    }
    
}