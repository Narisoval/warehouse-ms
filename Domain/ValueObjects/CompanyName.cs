using Domain.Primitives;
using FluentResults;

namespace Domain.ValueObjects;

public class CompanyName : ValueObject
{
    public string Value { get; set; }
    private static Range<int> LengthRange => Range<int>.Create(2, 50);

    private CompanyName(string companyName)
    {
        Value = companyName;
    }

    public static Result<CompanyName> From(string companyName)
    {
        if (companyName.Length < LengthRange.Min || companyName.Length > LengthRange.Max)
        {
            return new Result<CompanyName>()
                .WithError(new Error(
                    $"Length of {nameof(CompanyName)} " +
                    $"should be between {LengthRange.Min} and {LengthRange.Max}"));
        }

        return new CompanyName(companyName);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}