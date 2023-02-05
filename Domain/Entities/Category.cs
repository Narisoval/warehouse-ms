using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Category : Entity
{
    public CategoryName Name { get; set; }

    public IReadOnlyCollection<Product> Products { get; set; }

    private Category(Guid id, CategoryName? name) : base(id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public static Category Create(Guid id, CategoryName? name)
    {
        return new Category(id, name);
    }
}