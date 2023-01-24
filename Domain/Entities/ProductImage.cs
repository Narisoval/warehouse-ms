using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class ProductImage : Entity
{
    public Image Image { get; private set; }

    private ProductImage(Guid id, Image image) : base(id)
    {
        Image = image ?? throw new ArgumentNullException(nameof(image));
    }

    public static ProductImage Create(Guid id, Image image)
    {
        return new ProductImage(id, image);
    }
}