using Domain.Errors;
using Domain.Primitives;
using FluentResults;

namespace Domain.ValueObjects;

public sealed class Image : ValueObject 
{
    public string Value { get; }

    private Image(string imageUrl)
    {
        Value = imageUrl;
    } 
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static Result<Image> From(string? imageUrl)
    {
        var validationResult = Validate(imageUrl);
       
        if(validationResult.IsFailed)
            return new Result<Image>().WithErrors(validationResult.Errors);
        
        return new Image(imageUrl!);
    }

    private static Result Validate(string? imageUrl)
    {
        if (imageUrl == null)
            return new Result().WithError(new NullArgumentError(nameof(Image)));

        return ValidateImageUrl(imageUrl);
    }
    
    private static Result ValidateImageUrl(string url)
    {
        Result result = new Result();
        
        if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            result.WithError($"{url} is in the correct url format");

        bool hasValidUriScheme = url.StartsWith("http://") || url.StartsWith("https://");
        
        if(!hasValidUriScheme)
            result.WithError($"{url} doesn't have http or https url scheme");

        string[] validExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".svg" };

        bool endsWithRightExtension = validExtensions.Any(url.EndsWith);

        if (!endsWithRightExtension)
            return result.WithError($"{url} does not end with the correct image extension");

        return result;
    }
    
}