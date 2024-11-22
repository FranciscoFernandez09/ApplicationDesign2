using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain.HomeManagement;

namespace SmartHome.DataAccess.Repositories;

public class HomeRepository(DbContext context) : Repository<Home>(context)
{
    private readonly DbSet<Home> _home = context.Set<Home>();

    public override void Add(Home home)
    {
        try
        {
            _home.Add(home);

            // Not saving changes here because possible transactions issues
        }
        catch (Exception)
        {
            throw new DataAccessException("Error adding home to the database.");
        }
    }

    public override Home? Get(Expression<Func<Home, bool>>? predicate = null)
    {
        try
        {
            Expression<Func<Home, bool>> filter = predicate ?? (_ => true);
            Home? home = _home
                .Include(h => h.Owner)

                // HomeDevices
                .Include(h => h.Devices)
                .ThenInclude(hd => hd.Device)
                .ThenInclude(d => d.Images)
                .Include(h => h.Devices)
                .ThenInclude(hd => hd.Device)
                .ThenInclude(d => d.CompanyOwner)

                // HomeMembers
                .Include(h => h.Members)
                .ThenInclude(hm => hm.User)
                .Include(h => h.Members)
                .ThenInclude(hm => hm.Permissions)
                .FirstOrDefault(filter);

            return home;
        }
        catch (Exception)
        {
            throw new DataAccessException("Error getting home from the database.");
        }
    }

    public override List<Home> GetAll(Expression<Func<Home, bool>>? predicate = null)
    {
        try
        {
            Expression<Func<Home, bool>> filter = predicate ?? (_ => true);
            var homes = _home
                .Include(h => h.Owner)
                .Include(h => h.Devices)
                .Include(h => h.Members)
                .Where(filter)
                .ToList();

            return homes;
        }
        catch (Exception)
        {
            throw new DataAccessException("Error getting homes from the database.");
        }
    }
}
