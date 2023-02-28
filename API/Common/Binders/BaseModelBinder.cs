using System.Net.Mime;
using System.Text.Json;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Warehouse.API.DTO;

namespace Warehouse.API.Common.Binders;

public abstract class BaseModelBinder<TDto,TUpdateDto>: IModelBinder 
    where TDto : IEntityDto
{
    protected ModelBindingContext BindingContext = null!;
    protected abstract void ConvertDtoToEntity(TDto dto);
    protected abstract void ConvertUpdateDtoToEntity(TUpdateDto updateDto);
    
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
        if (TryGetIdFromRoute(out var guidId)) 
        {
            if (guidId != null) await BindFromDtoAsync(guidId.Value);
            return;
        }
        
        await BindFromUpdateDtoAsync();
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
    
    private async Task BindFromDtoAsync(Guid id)
    {
        var dto = (await BindingContext.HttpContext.Request
            .ReadFromJsonAsync<TDto>())!;
        
        dto.Id = id;

        ConvertDtoToEntity(dto);
    }

    private async Task BindFromUpdateDtoAsync()
    {
        var updateDto = (await BindingContext.HttpContext.Request
            .ReadFromJsonAsync<TUpdateDto>())!;
        
        ConvertUpdateDtoToEntity(updateDto);
    }
    
    protected void AddModelErrors(List<IError> errors, string key)
    {
        foreach (var error in errors)
        {
            BindingContext.ModelState.AddModelError(key,error.Message);    
        }
    }
}