using System.Text.Json;
using Domain.Entities;
using Domain.ValueObjects;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Warehouse.API.DTO.Category;

namespace Warehouse.API.Common.Bindings;

public class CategoryEntityModelBinder : BaseModelBinder
{
    public override async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        BindingContext = bindingContext;
        
        if (!CheckIfContentTypeIsJson())
            return;

        try
        {
            await BindCategoryEntityAsync();
        }
        catch (JsonException ex)
        {
            bindingContext.ModelState.AddModelError(
                "ObjectFormatError",
                $"{ex.InnerException?.Message} The following json element caused a problem: {ex.Path}");
        }
    }

    private async Task BindCategoryEntityAsync()
    {
        if (TryGetIdFromRoute(out var guidId)) 
        {
            if (guidId != null) await BindFromDtoAsync(guidId.Value);
            return;
        }
        
        await BindFromUpdateDtoAsync();
    }

    private async Task BindFromDtoAsync(Guid id)
    {
        CategoryDto categoryDto = (await BindingContext
            .HttpContext
            .Request
            .ReadFromJsonAsync<CategoryDto>())!;
        
        categoryDto.CategoryId = id;

        ConvertDtoToEntity(categoryDto);
    }

    private async Task BindFromUpdateDtoAsync()
    {
        CategoryUpdateDto categoryDto = (await BindingContext
            .HttpContext
            .Request
            .ReadFromJsonAsync<CategoryUpdateDto>())!;
        
        ConvertUpdateDtoToEntity(categoryDto);
    }

    private void ConvertDtoToEntity(CategoryDto categoryDto)
    {
        var categoryNameResult = CategoryName.From(categoryDto.Name);
        var id = categoryDto.CategoryId;
        
        if(!CheckIfResultsAreSuccessful(categoryNameResult))
            return;
        
        var categoryResult = Category.Create(id, categoryNameResult.Value);

        if (categoryResult.IsFailed)
        {
            AddModelErrors(categoryNameResult,"Category");
            return;
        }
        
        BindingContext.Result = ModelBindingResult.Success(categoryResult.Value);
    }

    private void ConvertUpdateDtoToEntity(CategoryUpdateDto categoryDto)
    {
        var categoryNameResult = CategoryName.From(categoryDto.Name);

        if(!CheckIfResultsAreSuccessful(categoryNameResult))
            return;
        
        var categoryResult = Category.Create(categoryNameResult.Value);

        if (categoryResult.IsFailed)
        {
            AddModelErrors(categoryNameResult,"Category");
            return;
        }
        
        BindingContext.Result = ModelBindingResult.Success(categoryResult.Value);
    }
    
    private bool CheckIfResultsAreSuccessful(Result<CategoryName> categoryNameResult)
    {
        if(categoryNameResult.IsFailed)
            AddModelErrors(categoryNameResult,"CategoryName");
            
        return BindingContext.ModelState.ErrorCount == 0;
    }
}