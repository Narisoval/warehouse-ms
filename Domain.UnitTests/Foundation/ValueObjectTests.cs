using Domain.Primitives;
using FluentAssertions;

namespace Domain.UnitTests.Foundation;

public class ValueObjectTests
{
    [Fact]
    public void Should_CompareObjectsEqual_When_TheirValuesAreEqual()
    {
        //Arrange
        var first = TestValueObject.From(5);
        var second = TestValueObject.From(5);
        
        //Act
        var areObjectsEqual = first.Equals(second);
        
        //Assert
        areObjectsEqual.Should().BeTrue();
        (first == second).Should().BeTrue();
        (first != second).Should().BeFalse();
    }

    [Fact]
    public void Should_NotCompareObjectsEqual_When_TheirValuesAreNotEqual()
    {
        //Arrange
        var first = TestValueObject.From(10);
        var second = TestValueObject.From(5);
        
        //Act
        var areObjectsEqual = first.Equals(second);
        
        //Assert
        areObjectsEqual.Should().BeFalse();
        (first == second).Should().BeFalse();
        (first != second).Should().BeTrue();
    }
    
    class TestValueObject : ValueObject
    {
        public int Value { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        private TestValueObject(int value)
        {
            Value = value;
        }
        
        public static TestValueObject From(int value)
        {
            return new TestValueObject(value);
        }
    }
}