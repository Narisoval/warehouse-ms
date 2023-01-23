using Domain.Validation;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.ValueObjects;

public class BrandDescriptionTests
{
    private static readonly Range<int> LengthRange = BrandDescription.GetRange();
    
    [Fact]
    public void Should_ThrowArgumentNullException_When_DescriptionIsNull()
    {
        //Arrange
        string? email = null; 
        
        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => BrandDescription.From(email));
    }

    [Fact]
    public void Should_ThrowArgumentOutOfRangeException_When_DescriptionLengthIsLessThanMinimum()
    {
        //Arrange
        string description = new string('a', LengthRange.Min-1);
        
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => BrandDescription.From(description));
    }

    [Fact]
    public void Should_ThrowArgumentOutOfRangeException_When_DescriptionLengthIsGreaterThanMaximum()
    {
        //Arrange
        string description = new string('a', LengthRange.Max+1);
        
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => BrandDescription.From(description));

    }

    [Fact]
    public void Should_FromBrandDescription_When_DescriptionIsValid()
    {
        // Arrange
        Random rnd = new Random();
        string description = new string('a',rnd.Next(LengthRange.Min,LengthRange.Max));

        // Act
        var sut = BrandDescription.From(description);

        //Assert
        sut.Value.Should().Be(description);
    }
}