using Domain.Errors;
using Domain.Primitives;
using Domain.ValueObjects;
using FluentResults;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Domain.Entities;

public class Category : Entity
{
    public CategoryName Name { get; private set; }
    
    public IReadOnlyCollection<Product>? Products { get; private set; }

    public Guid? ParentId { get; private set; }
    
    public Category? Parent { get; private set; }
    
    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<Category>? SubCategories { get; private set; } 

    public static Result<Category> Create(Guid id, CategoryName categoryName, Guid? parentId = null)
    {
        var result = new Result<Category>();

        if (id == Guid.Empty)
            result.WithError(new EmptyGuidError(nameof(Category)));
            
        
        return result.IsFailed ? result : new Category(id, categoryName, parentId);
    }

    public static Result<Category> Create(CategoryName name, Guid? parentId = null)
    {
        Guid id = Guid.NewGuid();
        return Create(id, name,parentId);
    }
    
    private Category(Guid id, CategoryName name, Guid? parentId = null) : base(id)
    {
        Name = name;
        ParentId = parentId;
    }
}