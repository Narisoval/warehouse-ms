using Domain.Primitives;

namespace Infrastructure.Interfaces;

public interface IRepository<TEntity> where  TEntity : Entity
{
   Task<TEntity?> Get(Guid id);
   
   Task<IEnumerable<TEntity>> GetAll();

   Task Add(TEntity entity);

   Task<bool> Update(TEntity entity);

   Task<bool> Remove(Guid id);
}