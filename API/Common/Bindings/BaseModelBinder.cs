using System.Net.Mime;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Warehouse.API.Common.Bindings;

public abstract class BaseModelBinder : IModelBinder
{
    public abstract Task BindModelAsync(ModelBindingContext bindingContext);

    protected ModelBindingContext BindingContext = null!;
    
    protected bool CheckIfContentTypeIsJson()
    {
        string? contentType = BindingContext.HttpContext.Request.ContentType;
        bool isJson = contentType?.StartsWith(MediaTypeNames.Application.Json) ?? false;

        if (!isJson)
        {
            var exception = new UnsupportedContentTypeException("Unsupported media type");
            BindingContext.ModelState.AddModelError("Exception", exception, BindingContext.ModelMetadata);
            return false;
        }

        return true;
    }
    
    protected bool TryGetIdFromRoute(out Guid? guidId)
    {
        bool hasIdAsRouteParameter = 
            BindingContext.ActionContext.RouteData.Values.TryGetValue("id", out var idValue);

        bool idIsValidGuid = Guid.TryParse(idValue?.ToString(), out var guid);
        
        if (hasIdAsRouteParameter && idIsValidGuid)
        {
            guidId = guid;
            return true;
        }
        
        if(hasIdAsRouteParameter && !idIsValidGuid)
            BindingContext.ModelState.AddModelError("id", $"The value '{idValue}' is not a valid GUID.");
        
        guidId = null;
        return false;
    }
    
    protected void AddModelErrors<T>(Result<T> result, string key)
    {
        foreach (var error in result.Errors)
        {
            BindingContext.ModelState.AddModelError(key,error.Message);    
        }
    }
}