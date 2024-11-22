using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain.HomeManagement;

namespace SmartHome.DataAccess.Repositories;

public class HomeDeviceRepository(DbContext context) : Repository<HomeDevice>(context)
{
    private readonly DbSet<HomeDevice> _homeDevices = context.Set<HomeDevice>();

    public override HomeDevice? Get(Expression<Func<HomeDevice, bool>>? predicate = null)
    {
        try
        {
            Expression<Func<HomeDevice, bool>> filter = predicate ?? (_ => true);
            HomeDevice? homeDevice = _homeDevices
                .Include(hd => hd.Home)
                .ThenInclude(h => h.Owner)
                .Include(hd => hd.Home)
                .ThenInclude(h => h.Members)
                .Include(hd => hd.Device)
                .Include(hd => hd.Room)
                .FirstOrDefault(filter);

            return homeDevice;
        }
        catch (Exception)
        {
            throw new DataAccessException("Error getting home device from the database.");
        }
    }
}
