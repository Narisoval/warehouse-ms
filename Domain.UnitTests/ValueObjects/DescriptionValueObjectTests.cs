using Domain.Exceptions;
using Domain.UnitTests.Fixtures.Generators;
using Domain.ValueObjects;

namespace Domain.UnitTests.ValueObjects;

public class DescriptionValueObjectTests
{
    [Theory]
    [MemberData(nameof(DescriptionsGenerator.GenerateDescriptions),MemberType = typeof(DescriptionsGenerator))]
    public void Should_ThrowException_When_DescriptionIsIncorrectLength(string description)
    {
        Assert.Throws<IncorrectLengthException>(() => Description.Create(description));
    }

    [Fact]
    public void Should_ThrowException_When_DescriptionIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Description.Create(null!));
    }
}