using System.Text.RegularExpressions;
using ValueOf;

namespace Domain.ValueObjects;

public sealed class Image : ValueOf<string,Image>
{
    protected override void Validate()
    {
        if (Value is null)
        {
            throw new ArgumentNullException(nameof(Value),"Image can't be null");
        }
        
        if (!IsValidImageUrl(Value))
        {
            throw new FormatException($"{Value} is not a valid image url");
        }

    }

    private static bool IsValidImageUrl(string imageUrl)
    {
        // This pattern is a really basic and allows some strange URLs like https:///.jpg 
        var pattern = @"(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|jpeg|png|svg|ico|bmp|webp)";
        var match = Regex.Match(imageUrl, pattern);
        return match.Success;
    }
    
}