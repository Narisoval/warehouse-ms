using Domain.Entities;
using Warehouse.API.DTO.ProductDtos;
using Warehouse.API.Messaging.Events.ProductEvents;

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

    public static ProductUpdatedEvent ToUpdatedEvent(this Product product)
    {
        return new ProductUpdatedEvent
        {
            Id = product.Id,
            Name = product.Name.Value,
            Description = product.Description.Value,
            MainImage = product.MainImage.Value,
            Images = product.Images?
                .Select(image => image.Image.Value).ToList(),
            FullPrice = product.FullPrice.Value,
            Discount = product.Sale.Value,
            Quantity = product.Quantity.Value,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId,
            BrandId = product.BrandId
        };
    }

    public static ProductCreatedEvent ToCreatedEvent(this Product product)
    {
        return new ProductCreatedEvent
        {
            Id = product.Id,
            Name = product.Name.Value,
            Description = product.Description.Value,
            MainImage = product.MainImage.Value,
            Images = product.Images?
                .Select(image => image.Image.Value).ToList(),
            FullPrice = product.FullPrice.Value,
            Discount = product.Sale.Value,
            Quantity = product.Quantity.Value,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId,
            BrandId = product.BrandId
        };
    }
}