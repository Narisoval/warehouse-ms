using System.Net.Mime;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace Warehouse.API.Helpers.Binders;

public class ContentTypeChecker
{
    private ModelBindingContext bindingContext;

    public ContentTypeChecker(ModelBindingContext bindingContext)
    {
        this.bindingContext = bindingContext;
    }
    
    public bool IsContentTypeJson()
    {
        string? contentType = bindingContext.HttpContext.Request.ContentType;
        bool isJson = contentType?.StartsWith(MediaTypeNames.Application.Json) ?? false;

        if (!isJson)
        {
            HandleIfContentTypeIsNotJson();
            return false;
        }

        return true;
    }
    
    private void HandleIfContentTypeIsNotJson()
    {
        var exception = new UnsupportedContentTypeException("Unsupported media type");
        bindingContext.ModelState.AddModelError("Exception", exception, bindingContext.ModelMetadata);
    }

    public void HandleJsonException(JsonException ex) 
    {
        bindingContext.ModelState.AddModelError(
            "ObjectFormatError",
            $"{ex.InnerException?.Message} The following json element caused this problem: {ex.Path}");
    }
}