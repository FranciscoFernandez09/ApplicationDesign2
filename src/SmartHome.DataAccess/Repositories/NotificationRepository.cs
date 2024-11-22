using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.DataAccess.Repositories;

public class NotificationRepository(DbContext context) : Repository<Notification>(context)
{
    private readonly DbSet<Notification> _notification = context.Set<Notification>();

    public override List<Notification> GetAll(Expression<Func<Notification, bool>>? predicate = null)
    {
        try
        {
            Expression<Func<Notification, bool>> filter = predicate ?? (_ => true);
            var notification = _notification
                .Include(n => n.HomeDevice)
                .ThenInclude(hd => hd.Device)
                .Where(filter)
                .ToList();

            return notification;
        }
        catch (Exception)
        {
            throw new DataAccessException("Error getting notifications from the database.");
        }
    }
}
