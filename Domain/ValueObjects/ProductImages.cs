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
        
        //This check is necessary for retrieving products for DB to work correctly.
        //Because when getting Products from db with ProductImages included,
        //EF initializes an empty list of ProductImage that is going through this validation logic.
        //So IF THIS CHECK WILL HAVE TO BE REMOVED IN THE FUTURE,
        //IT SHOULD BE MOVED TO SOME OTHER PLACE, like setter of Images property in Product.       
        
        if (Value.Count == 0)
            return;
        
        ValidateAmountOfMainImages();
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

    private void ValidateAmountOfMainImages()
    {
        int amountOfMainImages = Value.Count(image => image.IsMain);
        
        if (amountOfMainImages == 0)
            throw new NoMainImagesDomainException();
        
        if(amountOfMainImages > MaxAmountOfMainImages)
            throw new TooManyMainImagesDomainException(MaxAmountOfMainImages);
    }
}