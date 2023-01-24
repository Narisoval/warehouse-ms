using Domain.Primitives;

namespace Domain.Entities;

public class Category : Entity
{
    public string Name { get; private set; }

    private Category(Guid id, string name) : base(id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public static Category Create(Guid id, string name)
    {
        return new Category(id, name);
    }

    public void ChangeName(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}