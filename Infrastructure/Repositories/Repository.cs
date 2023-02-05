using System.Linq.Expressions;
using Domain.Primitives;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class Repository<TEntity,TContext> : IRepository<TEntity> where TEntity : Entity where TContext : DbContext
{
    protected readonly TContext Context;
    protected readonly DbSet<TEntity> Entities;

    public Repository(TContext context)
    {
        Context = context;
        Entities = context.Set<TEntity>();
    }

    public async Task<TEntity?> Get(Guid id)
    {
        return await Entities.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return await Entities.ToListAsync();
    }

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return Entities.Where(predicate);
    }

    public async Task Add(TEntity entity)
    {
        await Entities.AddAsync(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        Entities.AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
        Entities.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        Entities.RemoveRange(entities);
    }
}
