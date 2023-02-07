using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class ProductImagesFixture
{
    public static ProductImages GetTestProductImages()
    {
        List<ProductImage> productImagesList = new List<ProductImage>()
        {
            ProductImage.Create(Guid.NewGuid(), Image.From("https://imagesss/chair1.png"),true),
            ProductImage.Create(Guid.NewGuid(), Image.From("https://imagess/chair2.png"),false),
            ProductImage.Create(Guid.NewGuid(), Image.From("https://imagess/chair3.png"),false)
        };
        
        return ProductImages.From(productImagesList);
    }
}