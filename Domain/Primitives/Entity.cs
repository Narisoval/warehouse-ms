using System.ComponentModel;

namespace Domain.Primitives;
public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; }
    
    protected Entity(Guid? id)
    {
        if(id == Guid.Empty)
            throw new InvalidEnumArgumentException("Id should be a valid Guid");
        
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }
    
    //For EF
    protected Entity()
    {
    }

    public bool Equals(Entity? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Entity)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity? left, Entity? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity? left, Entity? right)
    {
        return !Equals(left, right);
    }
}