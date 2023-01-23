using ValueOf;


namespace Domain.ValueObjects;

public sealed class Email : ValueOf<string,Email>
{
    protected override void Validate()
    {
        if (Value == null)
        {
            throw new ArgumentNullException(nameof(Value));
        }

        if (!IsCorrectFormat(Value))
        {
            throw new FormatException($"{Value} is not a valid email");
        }
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