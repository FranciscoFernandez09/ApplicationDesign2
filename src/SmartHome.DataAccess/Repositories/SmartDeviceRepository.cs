using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain.SmartDevices;

namespace SmartHome.DataAccess.Repositories;

public class SmartDeviceRepository(DbContext context) : Repository<SmartDevice>(context)
{
    private readonly DbSet<SmartDevice> _smartDevices = context.Set<SmartDevice>();

    public override List<SmartDevice> GetAll(Expression<Func<SmartDevice, bool>>? predicate, int offset, int limit)
    {
        try
        {
            Expression<Func<SmartDevice, bool>> filter = predicate ?? (_ => true);
            var devices = _smartDevices
                .Include(d => d.CompanyOwner)
                .Include(d => d.Images)
                .Where(filter)
                .Skip(offset)
                .Take(limit)
                .ToList();

            return devices;
        }
        catch (Exception)
        {
            throw new DataAccessException("Error getting devices from the database.");
        }
    }
}
