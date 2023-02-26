using FluentResults;

namespace Domain.Errors;

public class IncorrectLengthError : Error
{
    public IncorrectLengthError(string name, int minLength, int maxLength)
        : base($"{name} length should be between {minLength} and {maxLength}")
    {
        
    }
}