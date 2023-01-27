using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Brand : Entity
{
    public BrandName Name { get; set; }
    public Image Image { get; set; }
    public BrandDescription Description { get; set; }

    private Brand(Guid id, BrandName? brandName, Image? brandImage, BrandDescription? description) : base(id)
    {
        Name = brandName ?? throw new ArgumentNullException(nameof(brandName));
        Image = brandImage ?? throw new ArgumentNullException(nameof(brandImage));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public static Brand Create(Guid id, BrandName? brandName, Image? brandImage, BrandDescription? description)
    {
        return new Brand(id, brandName, brandImage, description);
    }

}