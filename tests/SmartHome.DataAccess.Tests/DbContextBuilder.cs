using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SmartHome.DataAccess.Tests.Repositories;

namespace SmartHome.DataAccess.Tests;

internal abstract record DbContextBuilder
{
    private static readonly SqliteConnection Connection = new("Data Source=:memory:");

    public static RepositoryTest.TestDbContext BuildRepositoryTestDbContext()
    {
        DbContextOptions<RepositoryTest.TestDbContext> options =
            new DbContextOptionsBuilder<RepositoryTest.TestDbContext>().UseSqlite(Connection).Options;

        Connection.Open();

        var context = new RepositoryTest.TestDbContext(options);

        return context;
    }

    public static UserRepositoryTest.TestDbContext BuildUserRepositoryTestDbContext()
    {
        DbContextOptions<UserRepositoryTest.TestDbContext> options =
            new DbContextOptionsBuilder<UserRepositoryTest.TestDbContext>().UseSqlite(Connection).Options;

        Connection.Open();

        var context = new UserRepositoryTest.TestDbContext(options);

        return context;
    }

    public static CompanyRepositoryTest.TestDbContext BuildCompanyRepositoryTestDbContext()
    {
        DbContextOptions<CompanyRepositoryTest.TestDbContext> options =
            new DbContextOptionsBuilder<CompanyRepositoryTest.TestDbContext>().UseSqlite(Connection).Options;

        Connection.Open();

        var context = new CompanyRepositoryTest.TestDbContext(options);

        return context;
    }

    public static SmartDeviceRepositoryTest.TestDbContext BuildSmartDeviceRepositoryTestDbContext()
    {
        DbContextOptions<SmartDeviceRepositoryTest.TestDbContext> options =
            new DbContextOptionsBuilder<SmartDeviceRepositoryTest.TestDbContext>().UseSqlite(Connection)
                .Options;

        Connection.Open();

        var context = new SmartDeviceRepositoryTest.TestDbContext(options);

        return context;
    }

    public static HomeRepositoryTest.TestDbContext BuildHomeRepositoryTestDbContext()
    {
        DbContextOptions<HomeRepositoryTest.TestDbContext> options =
            new DbContextOptionsBuilder<HomeRepositoryTest.TestDbContext>().UseSqlite(Connection)
                .Options;

        Connection.Open();

        var context = new HomeRepositoryTest.TestDbContext(options);

        return context;
    }

    public static HomeDeviceRepositoryTest.TestDbContext BuildHomeDeviceRepositoryTestDbContext()
    {
        DbContextOptions<HomeDeviceRepositoryTest.TestDbContext> options =
            new DbContextOptionsBuilder<HomeDeviceRepositoryTest.TestDbContext>().UseSqlite(Connection)
                .Options;

        Connection.Open();

        var context = new HomeDeviceRepositoryTest.TestDbContext(options);

        return context;
    }

    public static NotificationRepositoryTest.TestDbContext BuildNotificationRepositoryTestDbContext()
    {
        DbContextOptions<NotificationRepositoryTest.TestDbContext> options =
            new DbContextOptionsBuilder<NotificationRepositoryTest.TestDbContext>().UseSqlite(Connection)
                .Options;

        Connection.Open();

        var context = new NotificationRepositoryTest.TestDbContext(options);

        return context;
    }

    public static HomeMemberRepositoryTest.TestDbContext BuildHomeMemberRepositoryTestDbContext()
    {
        DbContextOptions<HomeMemberRepositoryTest.TestDbContext> options =
            new DbContextOptionsBuilder<HomeMemberRepositoryTest.TestDbContext>().UseSqlite(Connection)
                .Options;

        Connection.Open();

        var context = new HomeMemberRepositoryTest.TestDbContext(options);

        return context;
    }
}
