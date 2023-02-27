using System.Text.Json;
using Domain.Entities;
using Domain.ValueObjects;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Warehouse.API.DTO.BrandDtos;

namespace Warehouse.API.Common.Bindings;

public class BrandEntityModelBinder : BaseModelBinder
{
    public override async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        BindingContext = bindingContext;
        
        if (!CheckIfContentTypeIsJson())
            return;

        try
        {
            await BindBrandEntityAsync();
        }
        catch (JsonException ex)
        {
            BindingContext.ModelState.AddModelError(
                "ObjectFormatError",
                $"{ex.InnerException?.Message} The following json element caused a problem: {ex.Path}");
        }
    }
    
    private async Task BindBrandEntityAsync()
    {
        // Check if the route has a guid "id" route parameter
        if (TryGetIdFromRoute(out var guidId))
        {
            if (guidId != null) await BindFromDtoAsync(guidId.Value);
            return;
        }
        
        await BindUpdateDtoAsync();
    }

    private async Task BindFromDtoAsync(Guid id)
    {
        BrandDto brandDto = (await BindingContext.HttpContext.Request
            .ReadFromJsonAsync<BrandDto>())!;

        brandDto.BrandId = id;
        ConvertDtoToEntity(brandDto);
    }


    private async Task BindUpdateDtoAsync()
    {
        BrandUpdateDto brandUpdateDto = (await BindingContext.HttpContext.Request
            .ReadFromJsonAsync<BrandUpdateDto>())!;
        ConvertUpdateDtoToEntity(brandUpdateDto);
    }

    private void ConvertDtoToEntity(BrandDto brandDto)
    {
        var brandNameResult = BrandName.From(brandDto.Name);
        var brandDescriptionResult = BrandDescription.From(brandDto.Description);
        var brandImageResult = Image.From(brandDto.Image);
        var id = brandDto.BrandId;

        if (!CheckIfResultsAreSuccessful(brandNameResult, brandDescriptionResult, brandImageResult))
            return;
        
        var brandResult = Brand.Create(
            id: id, 
            brandName: brandNameResult.Value,
            brandImage: brandImageResult.Value,
            brandDescription: brandDescriptionResult.Value);

        if (brandResult.IsFailed)
        {
            AddModelErrors(brandResult,"Brand");
            return;
        }
        
        BindingContext.Result = ModelBindingResult.Success(brandResult.Value);
    }

    private bool CheckIfResultsAreSuccessful(
        Result<BrandName> brandNameResult, 
        Result<BrandDescription> brandDescriptionResult, 
        Result<Image> brandImageResult)
    {
        if(brandNameResult.IsFailed)
            AddModelErrors(brandNameResult, "BrandName");
        
        if(brandDescriptionResult.IsFailed)
            AddModelErrors(brandDescriptionResult, "BrandDescription");
        
        if(brandImageResult.IsFailed)
            AddModelErrors(brandImageResult, "BrandImage");
            
        return BindingContext.ModelState.ErrorCount == 0;
    }

    private void ConvertUpdateDtoToEntity(BrandUpdateDto brandUpdateDto)
    {
        var brandNameResult = BrandName.From(brandUpdateDto.Name);
        var brandDescriptionResult = BrandDescription.From(brandUpdateDto.Description);
        var brandImageResult = Image.From(brandUpdateDto.Image);

        if (!CheckIfResultsAreSuccessful(brandNameResult, brandDescriptionResult, brandImageResult))
            return;
        
        var brand = Brand.Create(
            brandName: brandNameResult.Value,
            brandImage: brandImageResult.Value,
            brandDescription: brandDescriptionResult.Value);
        
        BindingContext.Result = ModelBindingResult.Success(brand.Value);
    }
}