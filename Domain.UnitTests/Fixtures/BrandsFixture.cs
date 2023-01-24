using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class BrandsFixture
{
    public static Brand GetTestBrand()
    {
        return Brand.Create(
            Guid.NewGuid(), BrandName.From("Adidas"),
            Image.From("http://Logo.png"), 
            BrandDescription.From("Fjsdkfjsdlkfjsldkjflksdjflksdjflksdjflksdjglkdfjglkdf"));
    }
}