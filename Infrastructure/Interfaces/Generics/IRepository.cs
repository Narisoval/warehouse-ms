using Domain.Primitives;

namespace Infrastructure.Interfaces.Generics;

public interface IRepository<TEntity> where  TEntity : Entity
{
   Task<TEntity?> Get(Guid id);
   
   Task Add(TEntity entity);

   Task<bool> Update(TEntity entity);

   Task<bool> Remove(Guid id);
}