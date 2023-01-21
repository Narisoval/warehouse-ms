using System.Dynamic;
using Domain.Exceptions;
using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; }

    public Email(string email)
    {
        Value = email;
    }

    public static Email Create(string email)
    {
        if (email == null)
        {
            throw new ArgumentNullException("Value", "Email can't be null");
        }

        if (!IsCorrectFormat(email))
        {
            throw new InvalidEmailException($"{email} is not a valid email");
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
        
        if (domainPartSplitByDot.Any(s => s.Length == 0))
            return false;
        
        return true;
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}