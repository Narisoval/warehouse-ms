using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.ValueObjects;

public class QuantityTest
{
    [Fact]
    public void Should_ThrowException_When_QuantityIsLessThanZero()
    {
        //Arrange
        var quantity = -1;

        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => Quantity.From(quantity));
    }

    [Fact]
    public void Should_Create_Quantity_WhenQuantityIsValid()
    {
        //Arrange
        var quantity = 200;
        
        //Act 
        var sut = Quantity.From(quantity);
        
        //Assert
        sut.Value.Should().Be(quantity);
    }
}