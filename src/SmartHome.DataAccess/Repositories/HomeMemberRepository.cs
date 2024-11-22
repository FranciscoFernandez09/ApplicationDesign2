using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain.HomeManagement;

namespace SmartHome.DataAccess.Repositories;

public class HomeMemberRepository(DbContext context) : Repository<HomeMember>(context)
{
    private readonly DbSet<HomeMember> _homeMembers = context.Set<HomeMember>();

    public override List<HomeMember> GetAll(Expression<Func<HomeMember, bool>>? predicate = null)
    {
        try
        {
            Expression<Func<HomeMember, bool>> filter = predicate ?? (_ => true);
            var homeMembers = _homeMembers
                .Include(hm => hm.User)
                .Include(hm => hm.Permissions)
                .Include(hm => hm.Home)
                .ThenInclude(h => h.Owner)
                .Where(filter)
                .ToList();

            return homeMembers;
        }
        catch (Exception)
        {
            throw new DataAccessException("Error getting home members from the database.");
        }
    }

    public override HomeMember? Get(Expression<Func<HomeMember, bool>>? predicate = null)
    {
        try
        {
            Expression<Func<HomeMember, bool>> filter = predicate ?? (_ => true);
            HomeMember? homeMember = _homeMembers
                .Include(hm => hm.User)
                .Include(hm => hm.Home)
                .ThenInclude(h => h.Owner)
                .FirstOrDefault(filter);

            return homeMember;
        }
        catch (Exception)
        {
            throw new DataAccessException("Error getting home member from the database.");
        }
    }
}
