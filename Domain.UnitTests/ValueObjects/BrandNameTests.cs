using Domain.Primitives;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.ValueObjects;

public class BrandNameTests
{
    private readonly Range<int> _lengthRange = new BrandName().LengthRange;

    [Fact]
    public void Should_ThrowException_When_BrandNameIsNull()
    {
        //Arrange
        string? brandName = null;
        
        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => BrandName.From(brandName));
    }

    [Fact]
    public void Should_ThrowException_When_BrandNameLengthIsLessThenMinimum()
    {
        //Arrange
        var brandName = new string('a', _lengthRange.Min - 1);

        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => BrandName.From(brandName));
    }
    
    [Fact]
    public void Should_ThrowException_When_BrandNameLengthIsMoreThanMaximum()
    {
        //Arrange
        var brandName = new string('a', _lengthRange.Max + 1);

        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => BrandName.From(brandName));
    }

    [Fact]
    public void Should_Create_BrandName_When_LengthIsValid()
    {
        //Arrange
        var rnd = new Random();
        var length = rnd.Next(_lengthRange.Min, _lengthRange.Max);
        var brandName = new string('a', length);
        
        //Act
        var sut = BrandName.From(brandName);

        //Assert
        sut.Value.Should().BeEquivalentTo(brandName);
    }
}