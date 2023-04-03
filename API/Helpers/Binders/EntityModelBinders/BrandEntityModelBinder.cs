using Domain.Entities;
using Domain.ValueObjects;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Warehouse.API.DTO.BrandDtos;
using Image = Domain.ValueObjects.Image;

namespace Warehouse.API.Helpers.Binders.EntityModelBinders;

public sealed class BrandEntityModelBinder : EntityModelBinder<BrandDto>
{
    protected override void ConvertDtoToEntity(BrandDto brandDto, Guid? id)
    {
        var brandNameResult = BrandName.From(brandDto.Name);
        var brandDescriptionResult = BrandDescription.From(brandDto.Description);
        var brandImageResult = Image.From(brandDto.Image);

        if (!CheckIfResultsAreSuccessful(brandNameResult, brandDescriptionResult, brandImageResult))
            return;

        Result<Brand> brandResult;
        if (id != null)
        {
            brandResult = Brand.Create(
                id: id.Value, 
                brandName: brandNameResult.Value,
                brandImage: brandImageResult.Value,
                brandDescription: brandDescriptionResult.Value);
        }
        else
        {
            brandResult = Brand.Create(
                brandName: brandNameResult.Value,
                brandImage: brandImageResult.Value,
                brandDescription: brandDescriptionResult.Value);
        }

        if (!CheckIfBrandIsCreatedSuccessfully(brandResult))
            return;
        
        BindingContext.Result = ModelBindingResult.Success(brandResult.Value);
    }

    private bool CheckIfResultsAreSuccessful(
        Result<BrandName> brandNameResult, 
        Result<BrandDescription> brandDescriptionResult, 
        Result<Image> brandImageResult)
    {
        if(brandNameResult.IsFailed)
            AddModelErrors(brandNameResult.Errors, "BrandName");
        
        if(brandDescriptionResult.IsFailed)
            AddModelErrors(brandDescriptionResult.Errors, "BrandDescription");
        
        if(brandImageResult.IsFailed)
            AddModelErrors(brandImageResult.Errors, "BrandImage");
            
        return BindingContext.ModelState.ErrorCount == 0;
    }
    
    private bool CheckIfBrandIsCreatedSuccessfully(Result<Brand> brandResult)
    {
        if(brandResult.IsFailed)
            AddModelErrors(brandResult.Errors,"Brand");
        
        return brandResult.IsSuccess;
    }
}