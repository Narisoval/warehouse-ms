using Domain.Entities;
using Domain.ValueObjects;
using FluentResults;
using Warehouse.API.DTO.ProductDtos;

namespace Warehouse.API.Helpers.Mapping;

public static class ProductImageMapping
{
    public static ProductImageDto ToDto(this ProductImage productImage)
    {
        return new ProductImageDto
        {
            Image = productImage.Image.Value,
            IsMain = productImage.IsMain,
        };
    }
    
    public static IList<ProductImageDto>? ToDtos(this IList<ProductImage>? productImages)
    {
        if (productImages == null)
        {
            return null;
        }
        
        List<ProductImageDto> productImageDtos = new();
        
        foreach (var productImage in productImages)
        {
            productImageDtos.Add(productImage.ToDto());
        }

        return productImageDtos;
    }
    
    public static ProductImage ToEntity(this ProductImageDto productImageDto)
    {
        return ProductImage.Create(
            Image.From(productImageDto.Image).Value,
            productImageDto.IsMain).Value;
    }
    
    public static IList<ProductImage>? ToEntities(this IList<ProductImageDto>? productImagesDtos)
    {
        if (productImagesDtos == null)
        {
            return null;
        }
        
        List<ProductImage> productImages = new();
        
        foreach (var productImage in productImagesDtos)
        {
            productImages.Add(productImage.ToEntity());
        }

        return productImages;
    }
    
    public static Result<ProductImages> ToProductImagesResult(this IList<ProductImageDto> productImagesDtos)
    {
        Result<ProductImages> productImagesResult = new();
        List<ProductImage> productImageEntities = new() ;
        
        
        foreach (var dto in productImagesDtos)
        {
            var imageResult = Image.From(dto.Image);
            if (imageResult.IsFailed)
            {
                productImagesResult.WithErrors(imageResult.Errors);
                continue;
            }
            productImageEntities.Add(ProductImage.Create(imageResult.Value,dto.IsMain).Value);
        }

        if (productImagesResult.IsFailed)
            return productImagesResult;
        
        return ProductImages.From(productImageEntities);
    }
}