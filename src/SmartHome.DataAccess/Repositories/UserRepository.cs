using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.DataAccess.Repositories;

public class UserRepository(DbContext context) : Repository<User>(context)
{
    private readonly DbSet<User> _users = context.Set<User>();

    public override User? Get(Expression<Func<User, bool>>? predicate = null)
    {
        try
        {
            Expression<Func<User, bool>> filter = predicate ?? (_ => true);
            User? user = _users
                .Include(u => u.Role)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefault(filter);

            return user;
        }
        catch (Exception)
        {
            throw new DataAccessException("Error getting user from the database.");
        }
    }

    public override List<User> GetAll(Expression<Func<User, bool>>? predicate, int offset, int limit)
    {
        try
        {
            Expression<Func<User, bool>> filter = predicate ?? (_ => true);
            var users = _users
                .Include(u => u.Role)
                .ThenInclude(r => r.Permissions)
                .Where(filter)
                .Skip(offset)
                .Take(limit)
                .ToList();

            return users;
        }
        catch (Exception)
        {
            throw new DataAccessException("Error getting users from the database.");
        }
    }
}
