using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Brand : Entity
{
    public string BrandName { get; private set; }
    public Image? BrandImage { get; private set; }
    public BrandDescription? Description { get; private set; }
    public Brand(Guid id, string brandName, Image? brandImage, BrandDescription? description) : base(id)
    {
        BrandName = brandName;
        BrandImage = brandImage;
        Description = description;
    }
    
    public Brand(Guid id, string brandName, Image brandImage) : base(id)
    {
        BrandName = brandName;
        BrandImage = brandImage;
    }
    
    public Brand(Guid id, string brandName, BrandDescription description) : base(id)
    {
        BrandName = brandName;
        Description = description;
    }
    
    public Brand(Guid id, string brandName) : base(id)
    {
        BrandName = brandName;
    }

    public void ChangeImage(Image image)
    {
        BrandImage = image;
    }
    
    public void RemoveImage()
    {
        BrandImage = null;
    }
    
    public void ChangeDescription(BrandDescription description)
    {
        Description = description;
    }
    
    public void RemoveDescription()
    {
        Description = null;
    }
}