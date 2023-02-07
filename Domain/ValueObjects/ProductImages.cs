using Domain.Entities;
using Domain.Exceptions;
using ValueOf;

namespace Domain.ValueObjects;

public class ProductImages : ValueOf<IList<ProductImage>,ProductImages>
{
    public const int MaxAmountOfMainImages = 1;

    protected override void Validate()
    {
        if (Value == null)
            throw new ArgumentNullException(nameof(Value), "Product images can't be null");
        
        int amountOfMainImages = Value.Count(image => image.IsMain);
        
        if (amountOfMainImages == 0)
            throw new NoMainImagesDomainException();
        
        if(amountOfMainImages > MaxAmountOfMainImages)
            throw new TooManyMainImagesDomainException(MaxAmountOfMainImages);
    }
    
    public ProductImage GetProductImageById(Guid id)
    {
        ProductImage? productImage = Value.FirstOrDefault(img => img.Id == id);
        
        if (productImage == null)
        {
            throw new ArgumentException("Image Id is not found in images of this product");
        }
        
        return productImage;
    }
}