using Domain.Errors;
using FluentResults;

namespace Domain.Primitives;
public abstract class RangedValueObject<TThis,TValue> : ValueObject 
    where TValue : struct, IComparable 
    where TThis : RangedValueObject<TThis,TValue>, new()
{
    public TValue Value { get; private set; }
    
    internal abstract Range<TValue> Range { get; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value; 
    }
    
    public static Result<TThis> From(TValue value)
    {
        var valueObject = new TThis();
        var result = valueObject.Validate(value);
        
        if (!result.IsSuccess) return result;
        
        valueObject.Value = value;
        return valueObject;
    }

    private Result<TThis> Validate(TValue value )
    {
        if (!Range.InRange(value))
            return new Result<TThis>().WithError(
                new OutOfRangeError(typeof(TThis).Name,Range.ToString()));
                
        return Result.Ok();
    }
}
