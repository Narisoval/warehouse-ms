using Domain.Errors;
using FluentResults;

namespace Domain.Primitives;

public abstract class LengthRestrictedValueObject<T> : ValueObject
    where T : LengthRestrictedValueObject<T>, new()
{
    internal abstract Range<int> LengthRange { get; }
    
    public string Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value; 
    }
    
    public static Result<T> From(string? value)
    {
        var valueObject = new T();
        var result = valueObject.Validate(value);
        
        if (!result.IsSuccess) return result;
        
        valueObject.Value = value!;
        return valueObject;
    }

    private Result<T> Validate(string? value)
    {
        if (value == null)
            return new Result().WithError(new NullArgumentError(typeof(T).Name));

        if (!LengthRange.InRange(value.Length))
            return new Result()
                .WithError(new IncorrectLengthError(typeof(T).Name,
                    LengthRange.Min, LengthRange.Max));
        
        return Result.Ok();
    }

}
