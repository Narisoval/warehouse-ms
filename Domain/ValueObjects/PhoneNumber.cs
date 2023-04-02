using Domain.Errors;
using Domain.Primitives;
using FluentResults;

namespace Domain.ValueObjects;
public sealed class PhoneNumber : ValueObject
{
    public string Value { get; }

    private const int MinDigits = 5;
    private const int MaxLength = 20;
    
    private PhoneNumber(string value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static Result<PhoneNumber> From(string? phoneNumber)
    {
        if (phoneNumber == null)
        {
            return new Result<PhoneNumber>().WithError(new NullArgumentError(nameof(PhoneNumber)));
        }

        if (!IsValid(phoneNumber))
        {
            return new Result<PhoneNumber>().WithError(new 
                Error($"{phoneNumber} is not a valid phone number number."));
        }

        return new Result<PhoneNumber>().WithValue(new PhoneNumber(phoneNumber));
    }

     private static bool IsValid(string phoneNumber)
    {
        // Ensure that the local number has at least MinDigits digits and is no longer than MaxLength symbols
        if (phoneNumber.Length > MaxLength || phoneNumber.Count(char.IsDigit) < MinDigits)
            return false;

        return true;
    }
}
