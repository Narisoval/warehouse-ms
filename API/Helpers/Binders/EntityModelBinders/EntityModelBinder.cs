using System.Text.Json;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Warehouse.API.Helpers.Extensions;

namespace Warehouse.API.Helpers.Binders.EntityModelBinders;

public abstract class EntityModelBinder<TDto>: IModelBinder 
{
    protected ModelBindingContext BindingContext = null!;
    protected abstract void ConvertDtoToEntity(TDto dto, Guid? id);
    
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        BindingContext = bindingContext;

        var contentTypeChecker = new ContentTypeChecker(BindingContext);
        
        if (!contentTypeChecker.IsContentTypeJson())
            return;
        
        try
        {
            await BindEntityAsync();
        }
        catch (JsonException ex)
        {
            contentTypeChecker.HandleJsonException(ex);
        }
    }    
    
    private async Task BindEntityAsync()
    {
        TryGetIdFromRoute(out var guidId);
        await BindFromDtoAsync(guidId);
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
        errors.AddModelErrors(BindingContext.ModelState, key);
    }
}