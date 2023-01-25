using System.ComponentModel;

namespace Domain.Primitives;
public abstract class Entity : IEquatable<Entity>
{
    protected Entity(Guid? id)
    {
        if(id == Guid.Empty)
            throw new InvalidEnumArgumentException("Id should be a valid Guid");
        
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }
    public Guid Id { get; }

    public static bool operator ==(Entity? first, Entity? second)
    {
        return first is not null && second is not null && first.Equals(second);
    }

    public static bool operator !=(Entity? first, Entity? second)
    {
        return !(first == second);
    }

    public bool Equals(Entity? other)
    {
        if (other == null)
            return false;
        
        if (other.GetType() == GetType())
            return false;
        
        return other.Id == Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (obj.GetType() != GetType())
            return false;

        if (obj is not Entity entity)
            return false;
        
        return entity.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() ;
    }
}