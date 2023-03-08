using Domain.Entities;
using Warehouse.API.DTO.ProductDtos;

namespace Warehouse.API.Helpers.Mapping;

public static class ProductMapping
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            ProductId = product.Id,
            Name = product.Name.Value,
            Quantity = product.Quantity.Value,
            FullPrice = product.FullPrice.Value,
            Description = product.Description.Value,
            MainImage = product.MainImage.Value,
            Images = product.Images?.Select(images => images.Image.Value).ToList(),
            Sale = product.Sale.Value,
            IsActive = product.IsActive,
            Brand = product.Brand?.ToUpdateDto(),
            Category = product.Category?.Name.Value,
            Provider = product.Provider?.ToUpdateDto()
        };
    }
}