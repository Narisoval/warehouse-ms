using System.Net.Mime;
using System.Text.Json;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Warehouse.API.Common.Binders;

public abstract class BaseModelBinder<TDto>: IModelBinder 
{
    protected ModelBindingContext BindingContext = null!;
    protected abstract void ConvertDtoToEntity(TDto dto, Guid? id);
    
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        BindingContext = bindingContext;
        
        if (!CheckIfContentTypeIsJson())
            return;

        try
        {
            await BindEntityAsync();
        }
        catch (JsonException ex)
        {
            HandleJsonException(ex);
        }
    }    
    
    private bool CheckIfContentTypeIsJson()
    {
        string? contentType = BindingContext.HttpContext.Request.ContentType;
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
        BindingContext.ModelState.AddModelError("Exception", exception, BindingContext.ModelMetadata);
    }
    
    private async Task BindEntityAsync()
    {
        TryGetIdFromRoute(out var guidId);
        await BindFromDtoAsync(guidId);
    }
    
    private void HandleJsonException(JsonException ex)
    {
        BindingContext.ModelState.AddModelError(
            "ObjectFormatError",
            $"{ex.InnerException?.Message} The following json element caused a problem: {ex.Path}");
    }
    
    private bool TryGetIdFromRoute(out Guid? guidId)
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

    private async Task BindFromDtoAsync(Guid? id = null)
    {
        var dto = (await BindingContext.HttpContext.Request
            .ReadFromJsonAsync<TDto>())!;
        
        ConvertDtoToEntity(dto,id);
    }

    protected void AddModelErrors(List<IError> errors, string key)
    {
        foreach (var error in errors)
        {
            BindingContext.ModelState.AddModelError(key,error.Message);    
        }
    }
}