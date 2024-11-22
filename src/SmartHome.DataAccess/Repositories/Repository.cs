using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.DataAccess.Repositories;

public class Repository<TEntity>(DbContext context) : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _entities = context.Set<TEntity>();

    public virtual void Add(TEntity entity)
    {
        try
        {
            _entities.Add(entity);
            context.SaveChanges();
        }
        catch (Exception)
        {
            throw new DataAccessException("Error adding entity to the database.");
        }
    }

    public void Update(TEntity entity)
    {
        try
        {
            _entities.Update(entity);
            context.SaveChanges();
        }
        catch (Exception)
        {
            throw new DataAccessException("Error updating entity in the database.");
        }
    }

    public void Delete(TEntity entity)
    {
        try
        {
            _entities.Remove(entity);
            context.SaveChanges();
        }
        catch (Exception)
        {
            throw new DataAccessException("Error deleting entity from the database.");
        }
    }

    public virtual bool Exists(Expression<Func<TEntity, bool>>? predicate = null)
    {
        try
        {
            Expression<Func<TEntity, bool>> filter = predicate ?? (_ => true);
            return _entities.Any(filter);
        }
        catch (Exception)
        {
            throw new DataAccessException("Error checking if entity exists in the database.");
        }
    }

    public virtual TEntity? Get(Expression<Func<TEntity, bool>>? predicate = null)
    {
        try
        {
            Expression<Func<TEntity, bool>> filter = predicate ?? (_ => true);
            TEntity? entity = _entities.FirstOrDefault(filter);

            return entity;
        }
        catch (Exception)
        {
            throw new DataAccessException("Error getting entity from the database.");
        }
    }

    public virtual List<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null)
    {
        try
        {
            return predicate == null ? _entities.ToList() : _entities.Where(predicate).ToList();
        }
        catch (Exception)
        {
            throw new DataAccessException("Error getting entities from the database.");
        }
    }

    public virtual List<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate, int offset, int limit)
    {
        try
        {
            return predicate == null
                ? _entities.Skip(offset).Take(limit).ToList()
                : _entities.Where(predicate).Skip(offset).Take(limit).ToList();
        }
        catch (Exception)
        {
            throw new DataAccessException("Error getting entities from the database.");
        }
    }
}
