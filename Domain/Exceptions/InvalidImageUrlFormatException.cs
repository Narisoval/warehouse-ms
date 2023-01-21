namespace Domain.Exceptions;

public class InvalidImageUrlFormatException : DomainException
{
    public InvalidImageUrlFormatException(string url) : base($"{url} is not a valid image URL")
    {
        ;
    }
}