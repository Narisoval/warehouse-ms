using Domain.Exceptions;
using Domain.Primitives;

namespace Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Image : ValueObject
{
    private readonly Uri _imageUrl;
    private Image(string imageUrl)
    {
        _imageUrl = new Uri(imageUrl);
    }

    public static Image Create(string imageUrl)
    {
        if (!IsValidImageUrl(imageUrl))
        {
            throw new InvalidImageUrlFormatException(imageUrl);
        }

        return new Image(imageUrl);
    }

    public string Value => _imageUrl.ToString();

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    private static bool IsValidImageUrl(string imageUrl)
    {
        // Regular expression to check if the URL is in a valid format
        var pattern = @"(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|gif|png)";
        var match = Regex.Match(imageUrl, pattern);
        return match.Success;
    }
}