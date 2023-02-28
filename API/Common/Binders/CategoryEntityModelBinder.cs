using Domain.Entities;
using Domain.ValueObjects;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Warehouse.API.DTO.Category;

namespace Warehouse.API.Common.Binders;

public sealed class CategoryEntityModelBinder : BaseModelBinder<CategoryDto,CategoryUpdateDto>
{
    protected override void ConvertDtoToEntity(CategoryDto categoryDto)
    {
        var categoryNameResult = CategoryName.From(categoryDto.Name);
        var id = categoryDto.Id;
        
        if(!CheckIfResultsAreSuccessful(categoryNameResult))
            return;
        
        var categoryResult = Category.Create(id, categoryNameResult.Value);
        
        if(!CheckIfCategoryIsCreatedSuccessfully(categoryResult))
            return;
        
        BindingContext.Result = ModelBindingResult.Success(categoryResult.Value);
    }

    protected override void ConvertUpdateDtoToEntity(CategoryUpdateDto categoryDto)
    {
        var categoryNameResult = CategoryName.From(categoryDto.Name);

        if(!CheckIfResultsAreSuccessful(categoryNameResult))
            return;
        
        var categoryResult = Category.Create(categoryNameResult.Value);
        
        if(!CheckIfCategoryIsCreatedSuccessfully(categoryResult))
            return;
        
        BindingContext.Result = ModelBindingResult.Success(categoryResult.Value);
    }

    private bool CheckIfResultsAreSuccessful(Result<CategoryName> categoryNameResult)
    {
        if(categoryNameResult.IsFailed)
            AddModelErrors(categoryNameResult.Errors,"CategoryName");
            
        return BindingContext.ModelState.ErrorCount == 0;
    }
    
    private bool CheckIfCategoryIsCreatedSuccessfully(Result<Category> categoryResult)
    {
        if (categoryResult.IsFailed)
            AddModelErrors(categoryResult.Errors,"Category");

        return categoryResult.IsFailed;

    }
}