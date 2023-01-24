using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Brand : Entity
{
    public string BrandName { get; private set; }
    public Image BrandImage { get; private set; }
    public BrandDescription Description { get; private set; }

    private Brand(Guid id, string? brandName, Image? brandImage, BrandDescription? description) : base(id)
    {
        BrandName = brandName ?? throw new ArgumentNullException(nameof(brandName));
        BrandImage = brandImage ?? throw new ArgumentNullException(nameof(brandImage));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public static Brand Create(Guid id, string brandName, Image? brandImage, BrandDescription? description)
    {
        return new Brand(id, brandName, brandImage, description);
    }

    public void ChangeImage(Image image)
    {
        BrandImage = image;
    }
    
    public void ChangeDescription(BrandDescription description)
    {
        Description = description;
    }
    
}