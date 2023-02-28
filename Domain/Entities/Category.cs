using Domain.Errors;
using Domain.Primitives;
using Domain.ValueObjects;
using FluentResults;

namespace Domain.Entities;

public class Category : Entity
{
    public CategoryName Name { get; set; }

    public IReadOnlyCollection<Product>? Products { get; set; }

    private Category(Guid id, CategoryName name) : base(id)
    {
        Name = name;
    }

    public static Result<Category> Create(Guid id, CategoryName? categoryName)
    {
        Result<Category> result = new Result<Category>();

        if (id == Guid.Empty)
            result.WithError(new EmptyGuidError(nameof(Category)));
            
        if (categoryName! == null!)
            result.WithError(new NullArgumentError(nameof(categoryName)));
        
        if (result.Errors.Count != 0)
            return result;
        
        return new Category(id, categoryName!);
    }

    public static Result<Category> Create(CategoryName? name)
    {
        Guid id = Guid.NewGuid();
        return Create(id, name);
    }
}