using Domain.Entities;
using Domain.ValueObjects;
using Warehouse.API.DTO;
using Warehouse.API.DTO.Product;

namespace Warehouse.API.Common.Mapping;

public static class ProductImageMappingConfig
{
    public static ProductImageDto ToDTo(this ProductImage productImage)
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
            productImageDtos.Add(productImage.ToDTo());
        }

        return productImageDtos;
    }
    
    public static ProductImage ToEntity(this ProductImageDto productImageDto)
    {
        return ProductImage.Create(
            Image.From(productImageDto.Image),
            productImageDto.IsMain
        );
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
}