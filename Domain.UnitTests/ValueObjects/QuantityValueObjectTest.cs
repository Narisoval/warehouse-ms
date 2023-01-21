using Domain.ValueObjects;

namespace Domain.UnitTests.ValueObjects;

public class QuantityValueObjectTest
{
    [Fact]
    public void Should_ThrowException_When_QuantityIsLessThanZero()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Quantity.Create(-1));
    }
}