using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SmartHome.BusinessLogic;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.EFCoreClasses;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.DataAccess.EFCoreClasses;

namespace SmartHome.DataAccess.Contexts;

public class SmartHomeDbContext(DbContextOptions<SmartHomeDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Home> Homes { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<SmartDevice> SmartDevices { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Session> Sessions { get; set; }

    public DbSet<MemberHomePermission> MemberHomePermission { get; set; }
    public DbSet<RoleSystemPermission> RoleSystemPermission { get; set; }
    public DbSet<HomeMemberNotification> HomeMemberNotifications { get; set; }

    public DbSet<DeviceImage> DeviceImages { get; set; }
    public DbSet<HomeDevice> HomeDevice { get; set; }
    public DbSet<HomeMember> HomeMembers { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<HomePermission> HomePermissions { get; set; }
    public DbSet<SystemPermission> SystemPermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigSchema(modelBuilder);
        ConfigSeedData(modelBuilder);
    }

    private void ConfigSchema(ModelBuilder modelBuilder)
    {
        // HomeDevice
        modelBuilder.Entity<HomeDevice>()
            .HasAlternateKey(hd => new { hd.HomeId, hd.DeviceId });

        modelBuilder.Entity<HomeDevice>()
            .HasOne(hd => hd.Home)
            .WithMany(h => h.Devices)
            .HasForeignKey(hd => hd.HomeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HomeDevice>()
            .HasOne(hd => hd.Device)
            .WithMany(d => d.HomeDevices)
            .HasForeignKey(hd => hd.DeviceId)
            .OnDelete(DeleteBehavior.Restrict);

        // HomeMember
        modelBuilder.Entity<HomeMember>()
            .HasAlternateKey(hm => new { hm.HomeId, hm.UserId });

        modelBuilder.Entity<HomeMember>()
            .HasOne(hm => hm.Home)
            .WithMany(h => h.Members)
            .HasForeignKey(hm => hm.HomeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HomeMember>()
            .HasOne(hm => hm.User)
            .WithMany(u => u.HomeMembers)
            .HasForeignKey(hm => hm.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // MemberHomePermission
        modelBuilder.Entity<HomeMember>()
            .HasMany(mhp => mhp.Permissions)
            .WithMany(p => p.HomeMembers)
            .UsingEntity<MemberHomePermission>();

        // RoleSystemPermission
        modelBuilder.Entity<Role>()
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity<RoleSystemPermission>();

        // HomeMemberNotification
        modelBuilder.Entity<Notification>()
            .HasMany(n => n.Members)
            .WithMany(m => m.Notifications)
            .UsingEntity<HomeMemberNotification>();

        modelBuilder
            .Entity<HomeMemberNotification>(entity =>
            {
                entity
                    .HasOne(h => h.Notification)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasOne(h => h.HomeMember)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });

        // SmartDevice
        modelBuilder.Entity<Camera>()
            .HasBaseType<SmartDevice>()
            .Property(c => c.HasExternalUse)
            .IsRequired();

        modelBuilder.Entity<Camera>()
            .Property(c => c.HasInternalUse)
            .IsRequired();

        modelBuilder.Entity<Camera>()
            .Property(c => c.MotionDetection)
            .IsRequired();

        modelBuilder.Entity<Camera>()
            .Property(c => c.PersonDetection)
            .IsRequired();
    }

    private void ConfigSeedData(ModelBuilder modelBuilder)
    {
        #region Permissions

        var createAdminPermission = new SystemPermission { Id = Constant.CreateAdminId, Name = "CreateAdmin" };
        var deleteAdminPermission = new SystemPermission { Id = Constant.DeleteAdminId, Name = "DeleteAdmin" };
        var createCompanyOwnerPermission =
            new SystemPermission { Id = Constant.CreateCompanyOwnerId, Name = "CreateCompanyOwner" };
        var getUsersPermission = new SystemPermission { Id = Constant.GetUsersId, Name = "GetUsers" };
        var getCompaniesPermission = new SystemPermission { Id = Constant.GetCompaniesId, Name = "GetCompanies" };
        var modifyAdminRolePermission =
            new SystemPermission { Id = Constant.AdminModifyRoleId, Name = "ModifyAdminRole" };
        var createCompanyPermission = new SystemPermission { Id = Constant.CreateCompanyId, Name = "CreateCompany" };
        var createDevicePermission = new SystemPermission { Id = Constant.CreateDeviceId, Name = "CreateDevice" };
        var modifyCompanyOwnerRolePermission =
            new SystemPermission { Id = Constant.CompanyOwnerModifyRoleId, Name = "ModifyCompanyOwnerRole" };
        var createHomePermission = new SystemPermission { Id = Constant.CreateHomeId, Name = "CreateHome" };
        var addHomePermission = new SystemPermission { Id = Constant.AddHomePermissionId, Name = "AddHomePermission" };
        var getNotificationsPermission =
            new SystemPermission { Id = Constant.GetNotificationsId, Name = "GetNotifications" };
        var addMemberPermission = new SystemPermission { Id = Constant.AddMemberId, Name = "AddMember" };
        var addSmartDevicePermission = new SystemPermission { Id = Constant.AddSmartDeviceId, Name = "AddSmartDevice" };
        var getHomeMembersPermission = new SystemPermission { Id = Constant.GetHomeMembersId, Name = "GetHomeMembers" };
        var getHomeDevicesPermission = new SystemPermission { Id = Constant.GetHomeDevicesId, Name = "GetHomeDevices" };
        var activateNotificationPermission =
            new SystemPermission { Id = Constant.ActivateNotificationId, Name = "ActivateMemberNotification" };
        var deactivateNotificationPermission =
            new SystemPermission { Id = Constant.DeactivateNotificationId, Name = "DeactivateMemberNotification" };
        var modifyHomeNamePermission =
            new SystemPermission { Id = Constant.ModifyHomeNameId, Name = "ModifyHomeName" };
        var modifyHomeDeviceNamePermission =
            new SystemPermission { Id = Constant.ModifyHomeDeviceNameId, Name = "ModifyHomeDeviceName" };
        var addRoomPermission = new SystemPermission { Id = Constant.AddRoomId, Name = "AddRoom" };
        var addDeviceToRoomPermission =
            new SystemPermission { Id = Constant.AddDeviceToRoomId, Name = "AddDeviceToRoom" };
        var getHomesPermission = new SystemPermission { Id = Constant.GetHomesId, Name = "GetHomes" };

        #endregion

        modelBuilder
            .Entity<SystemPermission>()
            .HasData(
                createAdminPermission, // AdminService
                deleteAdminPermission,
                createCompanyOwnerPermission,
                getUsersPermission,
                modifyAdminRolePermission,
                getCompaniesPermission,
                createCompanyPermission, // CompanyOwnerService
                createDevicePermission,
                modifyCompanyOwnerRolePermission,
                createHomePermission, // HomeOwnerService
                addHomePermission,
                getNotificationsPermission,
                addMemberPermission,
                addSmartDevicePermission,
                getHomeMembersPermission,
                getHomeDevicesPermission,
                activateNotificationPermission,
                deactivateNotificationPermission,
                modifyHomeNamePermission,
                modifyHomeDeviceNamePermission,
                addRoomPermission,
                addDeviceToRoomPermission,
                getHomesPermission);

        modelBuilder
            .Entity<HomePermission>()
            .HasData(
                new HomePermission { Id = Constant.AddSmartDeviceId, Name = "AddSmartDevice" },
                new HomePermission { Id = Constant.GetHomeDevicesId, Name = "GetHomeDevices" },
                new HomePermission { Id = Constant.AddDeviceToRoomId, Name = "AddDeviceToRoom" });

        #region Roles

        var adminRole = new Role { Id = Constant.AdminRoleId, Name = "Admin" };
        var homeOwnerRole = new Role { Id = Constant.HomeOwnerRoleId, Name = "HomeOwner" };
        var companyOwnerRole = new Role { Id = Constant.CompanyOwnerRoleId, Name = "CompanyOwner" };
        var adminHomeOwnerRole = new Role { Id = Constant.AdminHomeOwnerRoleId, Name = "AdminHomeOwner" };
        var companyAndHomeOwnerRole =
            new Role { Id = Constant.CompanyAndHomeOwnerRoleId, Name = "CompanyAndHomeOwner" };

        #endregion

        modelBuilder
            .Entity<Role>()
            .HasData(
                adminRole,
                homeOwnerRole,
                companyOwnerRole,
                adminHomeOwnerRole,
                companyAndHomeOwnerRole);

        modelBuilder
            .Entity<RoleSystemPermission>()
            .HasData(
                new RoleSystemPermission // Admin
                {
                    Id = Constant.CreateAdminSystemId,
                    RoleId = adminRole.Id,
                    PermissionId = createAdminPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.DeleteAdminSystemId,
                    RoleId = adminRole.Id,
                    PermissionId = deleteAdminPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CreateCompanyOwnerSystemId,
                    RoleId = adminRole.Id,
                    PermissionId = createCompanyOwnerPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.GetUsersSystemId,
                    RoleId = adminRole.Id,
                    PermissionId = getUsersPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.GetCompaniesSystemId,
                    RoleId = adminRole.Id,
                    PermissionId = getCompaniesPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AdminModifyRoleSystemId,
                    RoleId = adminRole.Id,
                    PermissionId = modifyAdminRolePermission.Id
                },
                new RoleSystemPermission // CompanyOwner
                {
                    Id = Constant.CreateCompanySystemId,
                    RoleId = companyOwnerRole.Id,
                    PermissionId = createCompanyPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CreateDeviceSystemId,
                    RoleId = companyOwnerRole.Id,
                    PermissionId = createDevicePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CompanyOwnerModifyRoleSystemId,
                    RoleId = companyOwnerRole.Id,
                    PermissionId = modifyCompanyOwnerRolePermission.Id
                },
                new RoleSystemPermission // HomeOwner
                {
                    Id = Constant.CreateHomeSystemId,
                    RoleId = homeOwnerRole.Id,
                    PermissionId = createHomePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AddHomePermissionSystemId,
                    RoleId = homeOwnerRole.Id,
                    PermissionId = addHomePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.GetNotificationsSystemId,
                    RoleId = homeOwnerRole.Id,
                    PermissionId = getNotificationsPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AddMemberSystemId,
                    RoleId = homeOwnerRole.Id,
                    PermissionId = addMemberPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AddSmartDeviceSystemId,
                    RoleId = homeOwnerRole.Id,
                    PermissionId = addSmartDevicePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.GetHomeMembersSystemId,
                    RoleId = homeOwnerRole.Id,
                    PermissionId = getHomeMembersPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.GetHomeDevicesSystemId,
                    RoleId = homeOwnerRole.Id,
                    PermissionId = getHomeDevicesPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.ActivateNotificationSystemId,
                    RoleId = homeOwnerRole.Id,
                    PermissionId = activateNotificationPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.DeactivateNotificationSystemId,
                    RoleId = homeOwnerRole.Id,
                    PermissionId = deactivateNotificationPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.ModifyHomeNameSystemId,
                    RoleId = homeOwnerRole.Id,
                    PermissionId = modifyHomeNamePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.ModifyHomeDeviceNameSystemId,
                    RoleId = homeOwnerRole.Id,
                    PermissionId = modifyHomeDeviceNamePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AddRoomSystemId,
                    RoleId = homeOwnerRole.Id,
                    PermissionId = addRoomPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AddDeviceToRoomSystemId,
                    RoleId = homeOwnerRole.Id,
                    PermissionId = addDeviceToRoomPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.GetHomesSystemId,
                    RoleId = homeOwnerRole.Id,
                    PermissionId = getHomesPermission.Id
                },
                new RoleSystemPermission // AdminHomeOwner
                {
                    Id = Constant.ACreateAdminSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = createAdminPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.ADeleteAdminSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = deleteAdminPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.ACreateCompanyOwnerSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = createCompanyOwnerPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AGetUsersSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = getUsersPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AGetCompaniesSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = getCompaniesPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.ACreateHomeSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = createHomePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AAddHomePermissionSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = addHomePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AGetNotificationsSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = getNotificationsPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AAddMemberSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = addMemberPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AAddSmartDeviceSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = addSmartDevicePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AGetHomeMembersSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = getHomeMembersPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AGetHomeDevicesSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = getHomeDevicesPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AActivateNotificationSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = activateNotificationPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.ADeactivateNotificationSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = deactivateNotificationPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AModifyHomeNameSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = modifyHomeNamePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AModifyHomeDeviceNameSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = modifyHomeDeviceNamePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AAddRoomSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = addRoomPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AAddDeviceToRoomSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = addDeviceToRoomPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.AGetHomesSystemId,
                    RoleId = adminHomeOwnerRole.Id,
                    PermissionId = getHomesPermission.Id
                },
                new RoleSystemPermission // CompanyAndHomeOwner
                {
                    Id = Constant.CCreateCompanySystemId,
                    RoleId = companyAndHomeOwnerRole.Id,
                    PermissionId = createCompanyPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CCreateDeviceSystemId,
                    RoleId = companyAndHomeOwnerRole.Id,
                    PermissionId = createDevicePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CCreateHomeSystemId,
                    RoleId = companyAndHomeOwnerRole.Id,
                    PermissionId = createHomePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CAddHomePermissionSystemId,
                    RoleId = companyAndHomeOwnerRole.Id,
                    PermissionId = addHomePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CGetNotificationsSystemId,
                    RoleId = companyAndHomeOwnerRole.Id,
                    PermissionId = getNotificationsPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CAddMemberSystemId,
                    RoleId = companyAndHomeOwnerRole.Id,
                    PermissionId = addMemberPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CAddSmartDeviceSystemId,
                    RoleId = companyAndHomeOwnerRole.Id,
                    PermissionId = addSmartDevicePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CGetHomeMembersSystemId,
                    RoleId = companyAndHomeOwnerRole.Id,
                    PermissionId = getHomeMembersPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CGetHomeDevicesSystemId,
                    RoleId = companyAndHomeOwnerRole.Id,
                    PermissionId = getHomeDevicesPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CActivateNotificationSystemId,
                    RoleId = companyAndHomeOwnerRole.Id,
                    PermissionId = activateNotificationPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CDeactivateNotificationSystemId,
                    RoleId = companyAndHomeOwnerRole.Id,
                    PermissionId = deactivateNotificationPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CModifyHomeNameSystemId,
                    RoleId = companyAndHomeOwnerRole.Id,
                    PermissionId = modifyHomeNamePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CModifyHomeDeviceNameSystemId,
                    RoleId = companyAndHomeOwnerRole.Id,
                    PermissionId = modifyHomeDeviceNamePermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CAddRoomSystemId,
                    RoleId = companyAndHomeOwnerRole.Id,
                    PermissionId = addRoomPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CAddDeviceToRoomSystemId,
                    RoleId = companyAndHomeOwnerRole.Id,
                    PermissionId = addDeviceToRoomPermission.Id
                },
                new RoleSystemPermission
                {
                    Id = Constant.CGetHomesSystemId,
                    RoleId = companyAndHomeOwnerRole.Id,
                    PermissionId = getHomesPermission.Id
                });

        var superAdminId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var createdAt = DateTime.ParseExact("2024-10-21 00:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        modelBuilder
            .Entity<User>()
            .HasData(
                new User
                {
                    Id = superAdminId,
                    Email = "superadmin@gmail.com",
                    Name = "SuperAdmin",
                    LastName = "SuperAdmin",
                    Password = "Super-Admin1",
                    RoleId = adminRole.Id,
                    HasCompany = false,
                    ProfileImage = null,
                    CreatedAt = createdAt
                });
    }

    public class SmartHomeDbContextFactory : IDesignTimeDbContextFactory<SmartHomeDbContext>
    {
        public SmartHomeDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SmartHomeDbContext>();

            const string connectionString =
                "Server=127.0.0.1;Database=SmartHomeDB;User Id=sa;Password=Password1;TrustServerCertificate=true;";

            optionsBuilder.UseSqlServer(connectionString);

            return new SmartHomeDbContext(optionsBuilder.Options);
        }
    }
}
