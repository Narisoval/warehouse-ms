using Domain.ValueObjects;

namespace Domain.UnitTests.ValueObjects;

public class PriceValueObjectTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(-300)]
    [InlineData(-20)]
    [InlineData(0.001)]
    [InlineData(0.009)]
    [InlineData(1_000_000_000_000)]
    private void Should_ThrowException_When_PriceIsIncorrect(decimal price)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Price.Create(price));
    }
}