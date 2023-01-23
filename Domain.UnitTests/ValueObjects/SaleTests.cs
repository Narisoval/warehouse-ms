using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.ValueObjects;

public class SaleTests
{
    [Fact]
    public void Should_ThrowException_WhenSaleIsLessThanZero()
    {
        //Arrange
        decimal sale = -1M;
        
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => Sale.From(sale));
    }
    
    [Fact]
    public void Should_ThrowException_WhenSaleIsMoreThan100()
    {
        //Arrange
        decimal sale = 101M;
        
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => Sale.From(sale));
    }
    
    [Theory]
    [InlineData(0)] 
    [InlineData(15)] 
    [InlineData(99.9)] 
    [InlineData(0.1)] 
    [InlineData(16.233)] 
    public void Should_CreateSale_When_SaleValueIsBetween0And100(decimal sale)
    {
        //Act 
        var sut = Sale.From(sale);
        
        //Assert
        sut.Value.Should().Be(sale);
    }
}