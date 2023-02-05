using System.Linq.Expressions;
using Domain.Primitives;

namespace Infrastructure.Interfaces;

public interface IRepository<TEntity> where  TEntity : Entity
{
   Task<TEntity?> Get(Guid id);
   Task<IEnumerable<TEntity>> GetAll();
   IEnumerable<TEntity> Find(Expression<Func<TEntity,bool>> predicate);

   Task Add(TEntity entity);
   void AddRange(IEnumerable<TEntity> entities);

   void Remove(TEntity entity);
   void RemoveRange(IEnumerable<TEntity> entities);
}