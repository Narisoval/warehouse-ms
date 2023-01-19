using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Brand : Entity
{
    public string BrandName { get; private set; }
    public string Image { get; private set; }
    public BrandDescription? Description { get; private set; }
    public Brand(Guid id, string brandName, string image, BrandDescription? description) : base(id)
    {
        BrandName = brandName;
        Image = image;
        Description = description;
    }
    
    public Brand(Guid id, string brandName, string image) : base(id)
    {
        BrandName = brandName;
        Image = image;
    }
}