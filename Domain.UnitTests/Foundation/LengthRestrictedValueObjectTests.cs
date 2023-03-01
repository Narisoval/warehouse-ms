using Domain.Primitives;
using FluentAssertions;

namespace Domain.UnitTests.Foundation;

public class LengthRestrictedValueObjectTests
{
    private const int MinLength = 5;
    private const int MaxLength = 70;

    public static IEnumerable<object[]> GetWrongValues()
    {
        yield return new object[] { new string('a', MaxLength+1) };
        yield return new object[] { new string('a', MinLength-1) };
        yield return new object[] {null!};
    }
    
    public static IEnumerable<object[]> GetRightValues()
    {
        yield return new object[] { new string('a', MaxLength) };
        yield return new object[] { new string('a', MinLength) };
        yield return new object[] { new string('a', MaxLength-MinLength) };
    }
    
    [Theory]
    [MemberData(nameof(GetWrongValues))]
    public void Should_ReturnFailedResult_When_ValueIsInvalid(string? value)
    {
        //Act
        var sut = TestValueObject.From(value);
        
        //Assert
        sut.IsFailed.Should().BeTrue();
        sut.Errors.Count.Should().Be(1);
    }
    
    [Theory]
    [MemberData(nameof(GetRightValues))]
    public void Should_CreateObjectCorrectly_When_ValueIsValid(string value)
    {
        //Act
        var sut = TestValueObject.From(value);
        
        //Assert
        sut.IsSuccess.Should().BeTrue();
        sut.Value.Value.Should().Be(value);
    }
    
    class TestValueObject : LengthRestrictedValueObject<TestValueObject>, IRanged<uint>
    {
        Range<uint> IRanged<uint>.Range { get; } = Range<uint>.Create(MinLength, MaxLength);
    }
}