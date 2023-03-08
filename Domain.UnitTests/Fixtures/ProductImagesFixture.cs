using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class ProductImagesFixture
{
    public static IReadOnlyCollection<ProductImage> GetTestProductImages()
    {
        List<ProductImage> productImagesList = new List<ProductImage>()
        {
            ProductImage.Create(Image.From("https://imagesss/chair1.png").Value).Value,
            ProductImage.Create(Image.From("https://imagess/chair2.png").Value).Value,
            ProductImage.Create(Image.From("https://imagess/chair3.png").Value).Value
        };

        return productImagesList;
    }
}