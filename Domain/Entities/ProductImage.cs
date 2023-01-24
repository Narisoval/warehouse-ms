using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class ProductImage : Entity
{
    public Image Image { get; private set; }
    
    public ProductImage(Guid id, Image image) : base(id)
    {
        Image = image;
    }
}