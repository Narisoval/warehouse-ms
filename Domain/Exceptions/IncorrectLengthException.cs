namespace Domain.Exceptions;

public class IncorrectLengthException : Exception
{
    public IncorrectLengthException(int minLength,int maxLength, string name) : 
        base($"Incorrect {name} length. {name} must be between {minLength} and {maxLength} characters long.") 
    {
        ;
    }
}