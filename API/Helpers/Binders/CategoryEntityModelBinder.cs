using Domain.ValueObjects;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Warehouse.API.DTO.CategoryDtos;
using Category = Domain.Entities.Category;

namespace Warehouse.API.Helpers.Binders;

public sealed class CategoryEntityModelBinder : BaseModelBinder<CategoryUpdateDto>
{
    protected override void ConvertDtoToEntity(CategoryUpdateDto categoryDto, Guid? id)
    {
        var categoryNameResult = CategoryName.From(categoryDto.Name);
        
        if(!CheckIfResultsAreSuccessful(categoryNameResult))
            return;

        Result<Category> categoryResult = id != null ? 
            Category.Create(id.Value, categoryNameResult.Value) : 
            Category.Create(categoryNameResult.Value);
        
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

        return categoryResult.IsSuccess;

    }
}