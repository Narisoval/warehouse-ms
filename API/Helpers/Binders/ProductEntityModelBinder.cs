using Domain.Entities;
using Domain.ValueObjects;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Warehouse.API.DTO.ProductDtos;

namespace Warehouse.API.Helpers.Binders;
public sealed class ProductEntityModelBinder : BaseModelBinder<ProductUpdateDto>
{
    protected override void ConvertDtoToEntity(ProductUpdateDto dto, Guid? id)
    {
        var productNameResult = ProductName.From(dto.Name);
        var quantityResult = Quantity.From(dto.Quantity);
        var priceResult = Price.From(dto.FullPrice);
        var descriptionResult = ProductDescription.From(dto.Description);
        var mainImageResult = Image.From(dto.MainImage);
        var imagesResult = ConvertImagesToResult(dto.Images);
        var saleResult = Sale.From(dto.Sale);
        
        if (!CheckIfResultsAreSuccessFull(productNameResult, quantityResult, 
                priceResult, descriptionResult, mainImageResult,imagesResult, saleResult))
            return;

        Result<Product> productResult;
        if (id == null)
        {
            productResult = Product.Create(
                productName: productNameResult.Value,
                quantity: quantityResult.Value,
                fullPrice: priceResult.Value,
                mainImage: mainImageResult.Value,
                images: imagesResult.Value,
                productDescription: descriptionResult.Value,
                isActive: dto.IsActive,
                sale: saleResult.Value,
                providerId: dto.ProviderId,
                brandId : dto.BrandId,
                categoryId : dto.CategoryId
            );
        }
        else
        {
             productResult = Product.Create(
                 id: id.Value,
                 productName: productNameResult.Value,
                 quantity: quantityResult.Value,
                 fullPrice: priceResult.Value,
                 mainImage: mainImageResult.Value,
                 images: imagesResult.Value,
                 productDescription: descriptionResult.Value,
                 isActive: dto.IsActive,
                 sale: saleResult.Value,
                 providerId: dto.ProviderId,
                 brandId : dto.BrandId,
                 categoryId : dto.CategoryId);           
        }

        if (!CheckIfProductIsCreatedSuccessfully(productResult))
            return;

        BindingContext.Result = ModelBindingResult.Success(productResult.Value);
    }

    private bool CheckIfResultsAreSuccessFull(Result<ProductName> productNameResult, Result<Quantity> quantityResult,
        Result<Price> priceResult, Result<ProductDescription> descriptionResult, Result<Image> mainImageResult,
        Result<IReadOnlyCollection<ProductImage>> imagesResult, Result<Sale> saleResult)
    {
        if(productNameResult.IsFailed)
            AddModelErrors(productNameResult.Errors,"ProductName");
        
        if(quantityResult.IsFailed)
            AddModelErrors(quantityResult.Errors,"Quantity");
        
        if(priceResult.IsFailed)
            AddModelErrors(priceResult.Errors,"Price");
        
        if(descriptionResult.IsFailed)
            AddModelErrors(descriptionResult.Errors,"ProductDescription");
        
        if(mainImageResult.IsFailed) 
            AddModelErrors(mainImageResult.Errors,"MainImage"); 
        
        if(imagesResult.IsFailed)
            AddModelErrors(imagesResult.Errors,"ProductImages");
        
        if(saleResult.IsFailed)
            AddModelErrors(saleResult.Errors,"Sale");
        
        return BindingContext.ModelState.ErrorCount == 0;
    }

    private bool CheckIfProductIsCreatedSuccessfully(Result<Product> productResult)
    {
        if (productResult.IsFailed)
            AddModelErrors(productResult.Errors, "Product");

        return productResult.IsSuccess;
    }

    public Result<IReadOnlyCollection<ProductImage>> ConvertImagesToResult(IReadOnlyCollection<string> images)
    {
        var productImages = new List<ProductImage>();
        var result = new Result<IReadOnlyCollection<ProductImage>>();
        //TODO clean this up
        foreach (var image in images)
        {
            var imageResult = Image.From(image);
            if (imageResult.IsFailed)
            {
                result.WithErrors(imageResult.Errors);
                continue;
            }

            var productImageResult = ProductImage.Create(imageResult.Value);
            
            if (productImageResult.IsFailed)
            {
                result.WithErrors(imageResult.Errors);
                continue;
            }
            
            productImages.Add(productImageResult.Value);
        }

        return result.WithValue(productImages);
    }
}