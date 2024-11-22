using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.DataAccess.Repositories;

public class CompanyRepository(DbContext context) : Repository<Company>(context)
{
    private readonly DbSet<Company> _companies = context.Set<Company>();

    public override void Add(Company company)
    {
        try
        {
            _companies.Add(company);

            // Not saving changes here because possible transactions issues
        }
        catch (Exception)
        {
            throw new DataAccessException("Error adding company to the database.");
        }
    }

    public override List<Company> GetAll(Expression<Func<Company, bool>>? predicate, int offset, int limit)
    {
        try
        {
            Expression<Func<Company, bool>> filter = predicate ?? (_ => true);
            var companies = _companies
                .Include(c => c.Owner)
                .Where(filter)
                .Skip(offset)
                .Take(limit)
                .ToList();

            return companies;
        }
        catch (Exception)
        {
            throw new DataAccessException("Error getting companies from the database.");
        }
    }
}
