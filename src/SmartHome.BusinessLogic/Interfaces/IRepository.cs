using System.Linq.Expressions;

namespace SmartHome.BusinessLogic.Interfaces;

public interface IRepository<TEntity>
    where TEntity : class
{
    void Add(TEntity entity);
    bool Exists(Expression<Func<TEntity, bool>> predicate);
    TEntity? Get(Expression<Func<TEntity, bool>>? predicate);
    void Delete(TEntity entity);
    List<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null);
    List<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate, int offset, int limit);
    void Update(TEntity entity);
}
