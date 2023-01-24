using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class BrandsFixture
{
    public static IEnumerable<Brand> GetBrands()
    {
        yield return Brand.Create(
            Guid.NewGuid(), "Adidas",
            Image.From("http://Logo.png"), 
            BrandDescription.From("Fjsdkfjsdlkfjsldkjflksdjflksdjflksdjflksdjglkdfjglkdf"));
    }
}