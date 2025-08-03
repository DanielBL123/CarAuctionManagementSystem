using CarAuction.Model.BaseEntities;
using System.Linq.Expressions;

namespace CarAuction.Sql.Repositories.Interfaces;
public interface IRepository<TEntity, T> where TEntity : class, IEntity<T>, new() where T : IComparable, IEquatable<T>
{
    Task<TEntity> AddAsync(TEntity entity);
    void Delete(TEntity entity);
    Task<TEntity> GetFirstAsync(T id);
    IQueryable<TEntity> AsQueryable(Expression<Func<TEntity, bool>> condition = null);
    void Update(TEntity entity);
    
    Task<int> SaveChangesAsync();

}
