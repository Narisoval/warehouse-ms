using Domain.UnitTests.Fixtures;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities.ProductTests;

public class ProductNameTests
{
    [Fact]
    public void Should_ChangeName_WhenCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var originalName = sut.Name;
        var newName = ProductName.From("New really detailed name");

        //Act 
        sut.ChangeName(newName);

        //Assert
        sut.Name.Should().Be(newName);
        sut.Name.Should().NotBe(originalName);
    }

    [Fact]
    public void Should_ThrowException_When_ChangeNameArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();

        //Act && Assert
        ProductName? newName = null;
        Assert.Throws<ArgumentNullException>(() => sut.ChangeName(newName));
    }
}