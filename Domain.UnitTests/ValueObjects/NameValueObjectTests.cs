using Domain.Exceptions;
using Domain.UnitTests.Fixtures;
using Domain.UnitTests.Fixtures.Generators;
using Domain.ValueObjects;

namespace Domain.UnitTests.ValueObjects;

public class NameValueObjectTests
{
    [Theory]
    [MemberData(nameof(ProductNameGenerator.GenerateProductNames),
        MemberType = typeof(ProductNameGenerator))]
    public void Should_ThrowException_When_NameIsIncorrectLength(string name)
    {
        Assert.Throws<IncorrectLengthException>(() => ProductName.Create(name));
    }
    
    [Fact]
    public void Should_ThrowException_When_NameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => ProductName.Create(null!));
    }
}