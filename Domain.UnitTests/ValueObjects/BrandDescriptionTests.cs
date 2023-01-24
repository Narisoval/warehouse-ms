using Domain.Primitives;
using Domain.ValueObjects;
using FluentAssertions;
using Random = System.Random;

namespace Domain.UnitTests.ValueObjects;

public class BrandDescriptionTests
{
    private readonly Range<int> _lengthRange = new BrandDescription().LengthRange;

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
        string description = new string('a', _lengthRange.Min-1);
        
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => BrandDescription.From(description));
    }

    [Fact]
    public void Should_ThrowArgumentOutOfRangeException_When_DescriptionLengthIsGreaterThanMaximum()
    {
        //Arrange
        string description = new string('a', _lengthRange.Max+1);
        
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => BrandDescription.From(description));

    }

    [Fact]
    public void Should_FromBrandDescription_When_DescriptionLengthIsValid()
    {
        // Arrange
        var rnd = new Random();
        var descriptionLength = rnd.Next(_lengthRange.Min, _lengthRange.Max);
        string description = new string('a',descriptionLength);

        // Act
        var sut = BrandDescription.From(description);

        //Assert
        sut.Value.Should().BeEquivalentTo(description);
    }
}