using Domain.Primitives;

namespace Domain.Entities;

public class Category : Entity
{
    public string Name { get; private set; }

    public Category(Guid id, string name) : base(id)
    {
        Name = name;
    }
}