using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class ProductImage : Entity
{
    public Image Image { get; internal set; }

    public bool IsMain { get; set; }
    
    public Product? Product { get; set; }
    
    public Guid ProductId { get; set; }

    private ProductImage(Guid id, Image image, bool isMain) : base(id)
    {
        Image = image ?? throw new ArgumentNullException(nameof(image));
        IsMain = isMain;
    }

    public static ProductImage Create(Guid id, Image image, bool isMain)
    {
        return new ProductImage(id, image,isMain);
    }
}