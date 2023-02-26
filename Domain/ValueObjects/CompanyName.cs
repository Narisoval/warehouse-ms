using Domain.Errors;
using Domain.Primitives;
using FluentResults;

namespace Domain.ValueObjects;

public class CompanyName : ValueObject
{
    public string Value { get; }
    private static Range<int> LengthRange => Range<int>.Create(2, 50);

    private CompanyName(string companyName)
    {
        Value = companyName;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static Result<CompanyName> From(string? companyName)
    {
        var validationResult = Validate(companyName);

        if (validationResult.IsFailed)
            return new Result<CompanyName>().WithErrors(validationResult.Errors);
        
        return new CompanyName(companyName!);
    }

    private static Result Validate(string? companyName)
    {
        if (companyName == null)
            return new Result().WithError(new NullArgumentError(nameof(CompanyName)));
        
        return ValidateLength(companyName);
    }

    private static Result ValidateLength(string companyName)
    {
        if (companyName.Length < LengthRange.Min || companyName.Length > LengthRange.Max)
            return new Result()
                .WithError(new IncorrectLengthError(nameof(companyName),LengthRange.Min,LengthRange.Max));
        
        return Result.Ok();
    }
}