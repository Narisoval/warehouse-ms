using Domain.Entities;
using Domain.UnitTests.Fixtures;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities.ProductTests;

public class ProductCategoryTests
{
    [Fact]
    public void Should_ThrowException_When_ChangeCategoryArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        Category? categoryToChangeTo = null;
        
        //Act & Assert
        Assert.Throws<ArgumentNullException>( () => sut.ChangeCategory(categoryToChangeTo));
    }
    
    [Fact]
    public void Should_ChangeCategory_When_Called()
    {
        //Arrange
        var testCategoryId = Guid.NewGuid();
        var testCategoryName = CategoryName.From("Electronics");
        var testCategory = Category.Create(testCategoryId, testCategoryName);
        var sut = ProductsFixture.GetTestProduct();
        
        //Act
        sut.ChangeCategory(testCategory); 
        
        //Assert
        sut.Category.Id.Should().Be(testCategoryId);
        sut.Category.Name.Should().Be(testCategoryName);
    }
}