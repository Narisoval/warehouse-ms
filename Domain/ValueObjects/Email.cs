using Domain.Primitives;
using FluentResults;


namespace Domain.ValueObjects;

public sealed class Email : ValueObject 
{
    public string Value { get; private init; }
    
    private Email(string email)
    {
        Value = email;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static Result<Email> From(string? email)
    {
        if (email == null)
        {
            return new Result<Email>().WithError(new Error("Email cannot be null"));
        }

        if (!IsCorrectFormat(email))
        {
            return new Result<Email>().WithError(new Error($"{email} is not a valid email"));
        }

        return new Email(email);
    }

    private static bool IsCorrectFormat(string email)
    {
        //checks if email has exactly one @ sign and domain part of email contains a dot
        // and both username and domain parts are not empty so a@b.c will a be valid email
        string[] splitByAtSign = email.Split("@");
        
        if (splitByAtSign.Length != 2 || splitByAtSign.Any(s => s.Length == 0))
            return false;

        string[] domainPartSplitByDot = splitByAtSign[1].Split(".");
        
        if (domainPartSplitByDot.Length < 2 || domainPartSplitByDot.Any(s => s.Length == 0))
            return false;
        
        return true;
    }

}