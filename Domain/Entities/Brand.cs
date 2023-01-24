using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Brand : Entity
{
    public BrandName Name { get; private set; }
    public Image Image { get; private set; }
    public BrandDescription Description { get; private set; }

    private Brand(Guid id, BrandName? brandName, Image? brandImage, BrandDescription? description) : base(id)
    {
        Name = brandName ?? throw new ArgumentNullException(nameof(brandName));
        Image = brandImage ?? throw new ArgumentNullException(nameof(brandImage));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public static Brand Create(Guid id, BrandName brandName, Image? brandImage, BrandDescription? description)
    {
        return new Brand(id, brandName, brandImage, description);
    }

    public void ChangeName(BrandName? brandName)
    {
        Name = brandName ?? throw new ArgumentNullException(nameof(brandName));
    }
    
    public void ChangeImage(Image? image)
    {
        Image = image ?? throw new ArgumentNullException(nameof(image));
    }
    
    public void ChangeDescription(BrandDescription? description)
    {
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }
    
}