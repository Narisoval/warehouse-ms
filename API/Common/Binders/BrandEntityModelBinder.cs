using Domain.Entities;
using Domain.ValueObjects;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Warehouse.API.DTO.BrandDtos;

namespace Warehouse.API.Common.Binders;

public sealed class BrandEntityModelBinder : BaseModelBinder<BrandDto,BrandUpdateDto>
{
    protected override void ConvertDtoToEntity(BrandDto brandDto)
    {
        var brandNameResult = BrandName.From(brandDto.Name);
        var brandDescriptionResult = BrandDescription.From(brandDto.Description);
        var brandImageResult = Image.From(brandDto.Image);
        var id = brandDto.Id;

        if (!CheckIfResultsAreSuccessful(brandNameResult, brandDescriptionResult, brandImageResult))
            return;
        
        var brandResult = Brand.Create(
            id: id, 
            brandName: brandNameResult.Value,
            brandImage: brandImageResult.Value,
            brandDescription: brandDescriptionResult.Value);

        if (!CheckIfBrandIsCreatedSuccessfully(brandResult))
            return;
        
        BindingContext.Result = ModelBindingResult.Success(brandResult.Value);
    }

    protected override void ConvertUpdateDtoToEntity(BrandUpdateDto brandUpdateDto)
    {
        var brandNameResult = BrandName.From(brandUpdateDto.Name);
        var brandDescriptionResult = BrandDescription.From(brandUpdateDto.Description);
        var brandImageResult = Image.From(brandUpdateDto.Image);

        if (!CheckIfResultsAreSuccessful(brandNameResult, brandDescriptionResult, brandImageResult))
            return;
        
        var brandResult = Brand.Create(
            brandName: brandNameResult.Value,
            brandImage: brandImageResult.Value,
            brandDescription: brandDescriptionResult.Value);
        
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
        return brandResult.IsFailed;
    }
}