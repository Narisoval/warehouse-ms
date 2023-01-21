using System.Net.Mime;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Brand : Entity
{
    public string BrandName { get; private set; }
    public Image BrandImage { get; private set; }
    public BrandDescription? Description { get; private set; }
    public Brand(Guid id, string brandName, Image brandImage, BrandDescription? description) : base(id)
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
}