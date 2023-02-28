using Domain.Errors;
using Domain.Primitives;
using FluentResults;

namespace Domain.ValueObjects;

public class CategoryName : ValueObject 
{
    internal static Range<int> LengthRange => Range<int>.Create(2, 30);
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public string Value { get; }

    private CategoryName(string value)
    {
        Value = value;
    }

    public static Result<CategoryName> From(string? categoryName)
    {
        return Validate(categoryName);
    }

    private static Result<CategoryName> Validate(string? categoryName)
    {
        if (categoryName == null)
            return new Result<CategoryName>().WithError(new NullArgumentError(nameof(CategoryName)));

        if (categoryName.Length < LengthRange.Min || categoryName.Length > LengthRange.Max)
            return new Result<CategoryName>().WithError(
                new IncorrectLengthError(nameof(CategoryName), 
                    LengthRange.Min, LengthRange.Max));
        
        return new CategoryName(categoryName);
    }
}