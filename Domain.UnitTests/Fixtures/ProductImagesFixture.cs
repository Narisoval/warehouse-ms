using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class ProductImagesFixture
{
    public static IList<ProductImage> GetProductImages()
    {
        List<ProductImage> productImages = new List<ProductImage>()
        {
            ProductImage.Create(Guid.NewGuid(), Image.From("https://imagesss/chair1.png")),
            ProductImage.Create(Guid.NewGuid(), Image.From("https://imagess/chair2.png")),
            ProductImage.Create(Guid.NewGuid(), Image.From("https://imagess/chair3.png"))
        };
        
        return productImages;
    }
}