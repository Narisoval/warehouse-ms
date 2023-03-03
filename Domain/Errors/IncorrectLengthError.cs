using FluentResults;

namespace Domain.Errors;

public class IncorrectLengthError : Error
{
    public IncorrectLengthError(string name, int minLength, int maxLength)
        : base($"{name} length should be between {minLength} and {maxLength}")
    {
        ;
    }

    public IncorrectLengthError(string name, uint minLength, uint maxLength) :
        base($"{name}'s length should be between {minLength} and {maxLength}")
    {
        ;
    }
}