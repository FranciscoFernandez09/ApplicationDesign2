using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.EFCoreClasses;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Interfaces.HomeManagement;
using SmartHome.BusinessLogic.Services;
using SmartHome.BusinessLogic.Services.HomeManagement;
using SmartHome.DataAccess.Contexts;
using SmartHome.DataAccess.EFCoreClasses;
using SmartHome.DataAccess.Repositories;
using SmartHome.WebApi.Filters;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder
    .Services
    .AddControllers(options =>
    {
        options.Filters.Add<ExceptionFilter>();
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

IServiceCollection services = builder.Services;
ConfigurationManager configuration = builder.Configuration;

var smartHomeConnectionString = configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(smartHomeConnectionString))
{
    throw new Exception("Missing SmartHome connection string");
}

services
    .AddDbContext<DbContext, SmartHomeDbContext>(
        options => options.UseSqlServer(smartHomeConnectionString));

services.AddScoped<IRepository<MemberHomePermission>, Repository<MemberHomePermission>>();
services.AddScoped<IRepository<RoleSystemPermission>, Repository<RoleSystemPermission>>();
services.AddScoped<IRepository<DeviceImage>, Repository<DeviceImage>>();
services.AddScoped<IRepository<HomePermission>, Repository<HomePermission>>();
services.AddScoped<IRepository<SystemPermission>, Repository<SystemPermission>>();
services.AddScoped<IRepository<HomeMemberNotification>, Repository<HomeMemberNotification>>();
services.AddScoped<IRepository<Role>, Repository<Role>>();
services.AddScoped<IRepository<Room>, Repository<Room>>();
services.AddScoped<IRepository<Session>, Repository<Session>>();

services.AddScoped<IRepository<HomeMember>, HomeMemberRepository>();
services.AddScoped<IRepository<Notification>, NotificationRepository>();
services.AddScoped<IRepository<HomeDevice>, HomeDeviceRepository>();
services.AddScoped<IRepository<Home>, HomeRepository>();
services.AddScoped<IRepository<SmartDevice>, SmartDeviceRepository>();
services.AddScoped<IRepository<Company>, CompanyRepository>();
services.AddScoped<IRepository<User>, UserRepository>();

services.AddScoped<IUnknownUserService, UserService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IHomeService, HomeService>();
services.AddScoped<IMemberService, MemberService>();
services.AddScoped<IHomeDeviceService, HomeDeviceService>();
services.AddScoped<IRoomService, RoomService>();
services.AddScoped<ICompanyOwnerService, CompanyOwnerService>();
services.AddScoped<IAdminService, AdminService>();
services.AddScoped<IDeviceActionService, DeviceActionService>();
services.AddScoped<ISessionService, SessionService>();
services.AddScoped<IModelValidatorService, ModelValidatorService>();
services.AddScoped<IDeviceImporterService, DeviceImporterService>();

// Configure CORS
services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
