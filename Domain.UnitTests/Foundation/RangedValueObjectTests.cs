using Domain.Primitives;
using FluentAssertions;

namespace Domain.UnitTests.Foundation;

public class RangedValueObjectTests
{
    private const int Min = 3;
    private const int Max = 100;

    public static IEnumerable<object[]> GetWrongValues()
    {
        yield return new object[] {Min - 1};
        yield return new object[] {Max + 1};
    }
    
    public static IEnumerable<object[]> GetRightValues()
    {
        yield return new object[] {Max - Min};
        yield return new object[] {Min};
        yield return new object[] { Max };
    }
    
    [Theory]
    [MemberData(nameof(GetWrongValues))]
    public void Should_ReturnFailedResult_When_NumberIsOutOfRange(int value)
    {
        //Act
        var sut = TestValueObject.From(value);
        
        //Assert
        sut.IsFailed.Should().BeTrue();
        sut.Errors.Count.Should().Be(1);
    }
    
    [Theory]
    [MemberData(nameof(GetRightValues))]
    public void Should_CreateObject_When_NumberIsInRange(int value)
    {
        //Act
        var sut = TestValueObject.From(value);
        
        //Assert
        sut.IsSuccess.Should().BeTrue();
        sut.Value.Value.Should().Be(value);
    }
    
    class TestValueObject : RangedValueObject<TestValueObject,int>, IRanged<int>
    {
        Range<int> IRanged<int>.Range => Range<int>.Create(Min, Max);
    }
}