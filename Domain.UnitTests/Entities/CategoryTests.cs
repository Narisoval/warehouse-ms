using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities;

public class CategoryTests
{
    [Fact]
    public void Should_ThrowException_When_ArgumentsAreNull()
    {
        //Arrange
        CategoryName? categoryName = null;
        var id = Guid.NewGuid();
        
        //Act & Asset
        Assert.Throws<ArgumentNullException>(() => Category.Create(id, categoryName));
    }
    [Fact]
    public void Should_CreateCategory_When_ArgumentsAreValid()
    {
        //Arrange
        CategoryName? categoryName = CategoryName.From("type");
        var id = Guid.NewGuid();
        
        //Act 
        var sut = Category.Create(id, categoryName);
        
        //Assert
        sut.Name.Should().BeEquivalentTo(categoryName);

    }
    
}