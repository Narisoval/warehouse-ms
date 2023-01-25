using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Brand : Entity
{
    public BrandName BrandName { get; set; }
    public Image BrandImage { get; set; }
    public BrandDescription Description { get; set; }

    private Brand(Guid id, BrandName? brandName, Image? brandImage, BrandDescription? description) : base(id)
    {
        BrandName = brandName ?? throw new ArgumentNullException(nameof(brandName));
        BrandImage = brandImage ?? throw new ArgumentNullException(nameof(brandImage));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public static Brand Create(Guid id, BrandName? brandName, Image? brandImage, BrandDescription? description)
    {
        return new Brand(id, brandName, brandImage, description);
    }

}