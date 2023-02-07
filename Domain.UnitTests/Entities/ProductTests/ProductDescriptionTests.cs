using Domain.UnitTests.Fixtures;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities.ProductTests;

public class ProductDescriptionTests
{
    [Fact]
    public void Should_ChangeDescription_WhenCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var newDescription =
            ProductDescription.From(
                "new better and more detailed description of a great productfds");

        //Act 
        sut.ChangeDescription(newDescription);

        //Assert
        sut.Description.Should().BeEquivalentTo(newDescription);
    }

    [Fact]
    public void Should_ThrowException_WhenChangeDescriptionArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        ProductDescription? newDescription = null;
        //Act && Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeDescription(newDescription));
    }
}