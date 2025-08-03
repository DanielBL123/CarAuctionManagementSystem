using CarAuction.Model.BaseEntities;
using CarAuction.Sql.Repositories.Interfaces;
using System.Linq.Expressions;

namespace CarAuction.Sql.Repositories.Classes;

public class Repository<TEntity, T, TContext>(TContext context) : IRepository<TEntity, T>
    where TEntity : class, IEntity<T>, new()
    where T : IComparable, IEquatable<T>
    where TContext : DbContext
{

    protected TContext _context = context;

    public readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async virtual Task<TEntity> AddAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Remove(entity);
    }

    public async virtual Task<TEntity> GetFirstAsync(T id)
    {
        if (id.Equals(default)) throw new ArgumentNullException(nameof(id));

        return await _dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public virtual IQueryable<TEntity> AsQueryable(Expression<Func<TEntity, bool>> condition = null!)
    {
        if (condition == null)
            return _dbSet.AsNoTracking();

        return _dbSet.AsNoTracking().Where(condition);
    }

    public virtual async Task<int> SaveChangesAsync() =>
        await _context.SaveChangesAsync(); 
    

    public void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Update(entity);
    }
}