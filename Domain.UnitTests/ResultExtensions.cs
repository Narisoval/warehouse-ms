using FluentAssertions;
using FluentResults;

namespace Domain.UnitTests;

public static class ResultExtensions
{
    public static void AssertIsFailed<T>(this Result<T> result, int errorsCount)
    {
        result.IsFailed.Should().BeTrue();
        result.Errors.Count.Should().Be(errorsCount);
    }
}