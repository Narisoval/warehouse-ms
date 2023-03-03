using Domain.Errors;
using FluentResults;

namespace Domain.Primitives;

public class LengthRestrictedValueObject<T> : ValueObject 
    where T : LengthRestrictedValueObject<T>, IRanged<uint>, new()
{
    public string Value { get; private set; }
    
    protected LengthRestrictedValueObject()
    {
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value; 
    }
    
    public static Result<T> From(string? value)
    {
        var valueObject = new T();
        var result = valueObject.Validate(value,valueObject);
        
        if (!result.IsSuccess) return result;
        
        valueObject.Value = value!;
        return valueObject;
    }

    private Result<T> Validate(string? value, IRanged<uint> ranged)
    {
        if (value == null)
            return new Result().WithError(new NullArgumentError(typeof(T).Name));

        if (!ranged.Range.InRange((uint)value.Length))
            return new Result()
                .WithError(new IncorrectLengthError(typeof(T).Name,
                    ranged.Range.Min, ranged.Range.Max));
        
        return Result.Ok();
    }
}
