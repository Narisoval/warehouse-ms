using System.Net.Mime;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Warehouse.API.Common.Mapping.Bindings;

public abstract class BaseModelBinder : IModelBinder
{
    public abstract Task BindModelAsync(ModelBindingContext bindingContext);
    
    protected bool CheckIfContentTypeIsJson(ModelBindingContext ctx)
    {
        string? contentType = ctx.HttpContext.Request.ContentType;
        bool isJson = contentType?.StartsWith(MediaTypeNames.Application.Json) ?? false;

        if (!isJson)
        {
            var exception = new UnsupportedContentTypeException("Unsupported media type");
            ctx.ModelState.AddModelError("Exception", exception, ctx.ModelMetadata);
            return false;
        }

        return true;
    }
    
    protected bool TryGetIdFromRoute(ModelBindingContext bindingContext, out Guid? guidId)
    {
        bool hasIdAsRouteParameter = 
            bindingContext.ActionContext.RouteData.Values.TryGetValue("id", out var idValue);

        bool idIsValidGuid = Guid.TryParse(idValue?.ToString(), out var guid);
        
        if (hasIdAsRouteParameter && idIsValidGuid)
        {
            guidId = guid;
            return true;
        }
        
        if(hasIdAsRouteParameter && !idIsValidGuid)
            bindingContext.ModelState.AddModelError("id", $"The value '{idValue}' is not a valid GUID.");
        
        guidId = null;
        return false;
    }
}