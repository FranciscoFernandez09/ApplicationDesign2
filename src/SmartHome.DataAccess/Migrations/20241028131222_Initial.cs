using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartHome.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasCompany = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleSystemPermission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleSystemPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleSystemPermission_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleSystemPermission_SystemPermissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "SystemPermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Homes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressStreet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressNumber = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<int>(type: "int", nullable: false),
                    Longitude = table.Column<int>(type: "int", nullable: false),
                    MaxMembers = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembersCount = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Homes_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_Sessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmartDevices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    HasExternalUse = table.Column<bool>(type: "bit", nullable: true),
                    HasInternalUse = table.Column<bool>(type: "bit", nullable: true),
                    MotionDetection = table.Column<bool>(type: "bit", nullable: true),
                    PersonDetection = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartDevices_Companies_CompanyOwnerId",
                        column: x => x.CompanyOwnerId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShouldNotify = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeMembers_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomeMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmartDeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceImages_SmartDevices_SmartDeviceId",
                        column: x => x.SmartDeviceId,
                        principalTable: "SmartDevices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MemberHomePermission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberHomePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberHomePermission_HomeMembers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "HomeMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberHomePermission_HomePermissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "HomePermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeDevice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsConnected = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeDevice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeDevice_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomeDevice_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HomeDevice_SmartDevices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "SmartDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Event = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    HomeDeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_HomeDevice_HomeDeviceId",
                        column: x => x.HomeDeviceId,
                        principalTable: "HomeDevice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeMemberNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomeMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeMemberNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeMemberNotifications_HomeMembers_HomeMemberId",
                        column: x => x.HomeMemberId,
                        principalTable: "HomeMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HomeMemberNotifications_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "HomePermissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("45678901-4567-4567-4567-1234567890ab"), "AddSmartDevice" },
                    { new Guid("67890123-6789-6789-6789-1234567890ab"), "GetHomeDevices" },
                    { new Guid("b3eee8dd-4c5f-4b8e-9a2e-1f3b5d6a7c8e"), "AddDeviceToRoom" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("12345678-1234-1234-1234-1234567890ab"), "Admin" },
                    { new Guid("23456789-2345-2345-2345-1234567890ab"), "CompanyOwner" },
                    { new Guid("34567890-3456-3456-3456-1234567890ab"), "HomeOwner" },
                    { new Guid("41111111-4567-4567-4567-123456789aab"), "CompanyAndHomeOwner" },
                    { new Guid("45678901-4567-4567-4567-123456789aab"), "AdminHomeOwner" }
                });

            migrationBuilder.InsertData(
                table: "SystemPermissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("01234567-0123-0123-0123-1234567890ab"), "ModifyCompanyOwnerRole" },
                    { new Guid("11111111-1111-1111-1111-1234567890ab"), "CreateAdmin" },
                    { new Guid("12345678-1234-1234-1234-1234567890ab"), "AddHomePermission" },
                    { new Guid("12345678-1234-1234-1234-1234567aaaab"), "ModifyHomeName" },
                    { new Guid("22222222-2222-2222-2222-1234567890ab"), "DeleteAdmin" },
                    { new Guid("23456789-2345-2345-2345-1234567890ab"), "GetNotifications" },
                    { new Guid("33333333-3333-3333-3333-1234567890ab"), "CreateCompanyOwner" },
                    { new Guid("34567890-3456-3456-3456-1234567890ab"), "AddMember" },
                    { new Guid("44444444-4444-4444-4444-1234567890ab"), "GetUsers" },
                    { new Guid("45678901-4567-4567-4567-1234567890ab"), "AddSmartDevice" },
                    { new Guid("55555555-5555-5555-5555-1234567890ab"), "GetCompanies" },
                    { new Guid("56789012-5678-5678-5678-1234567890ab"), "GetHomeMembers" },
                    { new Guid("66666666-6666-6666-6666-1234567890ab"), "CreateCompany" },
                    { new Guid("67890123-6789-6789-6789-1234567890ab"), "GetHomeDevices" },
                    { new Guid("77777777-7777-7777-7777-1234567890ab"), "CreateDevice" },
                    { new Guid("78901234-7890-7890-7890-1234567890ab"), "ActivateMemberNotification" },
                    { new Guid("89012345-8901-8901-8901-1234567890ab"), "DeactivateMemberNotification" },
                    { new Guid("90123456-9012-9012-9012-1234567890ab"), "ModifyAdminRole" },
                    { new Guid("99999999-9999-9999-9999-1234567890ab"), "CreateHome" },
                    { new Guid("b3e1a8d2-4c5f-4b8e-9a2e-1f3b5d6a7c8e"), "ModifyHomeDeviceName" },
                    { new Guid("b3eee8d2-4c5f-4b8e-9a2e-1f3b5d6a7c8e"), "AddRoom" },
                    { new Guid("b3eee8dd-4c5f-4b8e-9a2e-1f3b5d6a7c8e"), "AddDeviceToRoom" }
                });

            migrationBuilder.InsertData(
                table: "RoleSystemPermission",
                columns: new[] { "Id", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("08129fcb-bb32-4440-9d6d-daf74040c137"), new Guid("11111111-1111-1111-1111-1234567890ab"), new Guid("12345678-1234-1234-1234-1234567890ab") },
                    { new Guid("08129fcb-bb32-4440-aaaa-daf74040c137"), new Guid("11111111-1111-1111-1111-1234567890ab"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("0b66a103-82fe-4b3c-8046-e7c09f9ff094"), new Guid("22222222-2222-2222-2222-1234567890ab"), new Guid("12345678-1234-1234-1234-1234567890ab") },
                    { new Guid("0b66a103-82fe-4b3c-aaaa-e7c09f9ff094"), new Guid("22222222-2222-2222-2222-1234567890ab"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("10af3fc3-7a3f-4c21-aaaa-2e15f231caf3"), new Guid("33333333-3333-3333-3333-1234567890ab"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("10af3fc3-7a3f-4c21-b535-2e15f231caf3"), new Guid("33333333-3333-3333-3333-1234567890ab"), new Guid("12345678-1234-1234-1234-1234567890ab") },
                    { new Guid("13fbcb26-eccc-41cb-a4d0-203c8039aeab"), new Guid("44444444-4444-4444-4444-1234567890ab"), new Guid("12345678-1234-1234-1234-1234567890ab") },
                    { new Guid("13fbcb26-eccc-41cb-aaaa-203c8039aeab"), new Guid("44444444-4444-4444-4444-1234567890ab"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("5030f84f-e0fb-43ba-aaaa-3363dccf213d"), new Guid("55555555-5555-5555-5555-1234567890ab"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("5030f84f-e0fb-43ba-b3b8-3363dccf213d"), new Guid("55555555-5555-5555-5555-1234567890ab"), new Guid("12345678-1234-1234-1234-1234567890ab") },
                    { new Guid("5ee2f16e-8c00-494a-98b4-ca24e354ec35"), new Guid("66666666-6666-6666-6666-1234567890ab"), new Guid("23456789-2345-2345-2345-1234567890ab") },
                    { new Guid("5ee2f16e-8c00-494a-cccc-ca24e354ec35"), new Guid("66666666-6666-6666-6666-1234567890ab"), new Guid("41111111-4567-4567-4567-123456789aab") },
                    { new Guid("8bd3fb56-6430-48e1-b955-8b6fcd7f6544"), new Guid("77777777-7777-7777-7777-1234567890ab"), new Guid("23456789-2345-2345-2345-1234567890ab") },
                    { new Guid("8bd3fb56-6430-48e1-cccc-8b6fcd7f6544"), new Guid("77777777-7777-7777-7777-1234567890ab"), new Guid("41111111-4567-4567-4567-123456789aab") },
                    { new Guid("9f407a6c-95c2-4fee-aaaa-4a8aeb8181e9"), new Guid("99999999-9999-9999-9999-1234567890ab"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("9f407a6c-95c2-4fee-bdc6-4a8aeb8181e9"), new Guid("99999999-9999-9999-9999-1234567890ab"), new Guid("34567890-3456-3456-3456-1234567890ab") },
                    { new Guid("9f407a6c-95c2-4fee-cccc-4a8aeb8181e9"), new Guid("99999999-9999-9999-9999-1234567890ab"), new Guid("41111111-4567-4567-4567-123456789aab") },
                    { new Guid("a01983cf-6fc8-479f-9eba-7c0647e9cd5f"), new Guid("12345678-1234-1234-1234-1234567890ab"), new Guid("34567890-3456-3456-3456-1234567890ab") },
                    { new Guid("a01983cf-6fc8-479f-aaaa-7c0647e9cd5f"), new Guid("12345678-1234-1234-1234-1234567890ab"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("a01983cf-6fc8-479f-cccc-7c0647e9cd5f"), new Guid("12345678-1234-1234-1234-1234567890ab"), new Guid("41111111-4567-4567-4567-123456789aab") },
                    { new Guid("a6a4097e-eb63-41ba-9d57-c7df777e3218"), new Guid("23456789-2345-2345-2345-1234567890ab"), new Guid("34567890-3456-3456-3456-1234567890ab") },
                    { new Guid("a6a4097e-eb63-41ba-aaaa-c7df777e3218"), new Guid("23456789-2345-2345-2345-1234567890ab"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("a6a4097e-eb63-41ba-cccc-c7df777e3218"), new Guid("23456789-2345-2345-2345-1234567890ab"), new Guid("41111111-4567-4567-4567-123456789aab") },
                    { new Guid("b3eeeed2-4454-4b8e-9a2e-1f3b5d6a7c8e"), new Guid("b3eee8dd-4c5f-4b8e-9a2e-1f3b5d6a7c8e"), new Guid("34567890-3456-3456-3456-1234567890ab") },
                    { new Guid("b3eeeed2-4454-4b8e-aaaa-1f3b5d6a7c8e"), new Guid("b3eee8dd-4c5f-4b8e-9a2e-1f3b5d6a7c8e"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("b3eeeed2-4454-4b8e-cccc-1f3b5d6a7c8e"), new Guid("b3eee8dd-4c5f-4b8e-9a2e-1f3b5d6a7c8e"), new Guid("41111111-4567-4567-4567-123456789aab") },
                    { new Guid("b3eeeed2-4c5f-4b8e-9a2e-1f3b5d6a7c8e"), new Guid("b3eee8d2-4c5f-4b8e-9a2e-1f3b5d6a7c8e"), new Guid("34567890-3456-3456-3456-1234567890ab") },
                    { new Guid("b3eeeed2-4c5f-4b8e-aaaa-1f3b5d6a7c8e"), new Guid("b3eee8d2-4c5f-4b8e-9a2e-1f3b5d6a7c8e"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("b3eeeed2-4c5f-4b8e-cccc-1f3b5d6a7c8e"), new Guid("b3eee8d2-4c5f-4b8e-9a2e-1f3b5d6a7c8e"), new Guid("41111111-4567-4567-4567-123456789aab") },
                    { new Guid("b65f7a2f-a549-4fa2-974d-d6e073707ded"), new Guid("34567890-3456-3456-3456-1234567890ab"), new Guid("34567890-3456-3456-3456-1234567890ab") },
                    { new Guid("b65f7a2f-a549-4fa2-aaaa-d6e073707ded"), new Guid("34567890-3456-3456-3456-1234567890ab"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("b65f7a2f-a549-4fa2-cccc-d6e073707ded"), new Guid("34567890-3456-3456-3456-1234567890ab"), new Guid("41111111-4567-4567-4567-123456789aab") },
                    { new Guid("bbb1a8d2-4c5f-4b8e-9a2e-1f3b5d6a7c8e"), new Guid("b3e1a8d2-4c5f-4b8e-9a2e-1f3b5d6a7c8e"), new Guid("34567890-3456-3456-3456-1234567890ab") },
                    { new Guid("bbb1a8d2-4c5f-4b8e-aaaa-1f3b5d6a7c8e"), new Guid("b3e1a8d2-4c5f-4b8e-9a2e-1f3b5d6a7c8e"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("bbb1a8d2-4c5f-4b8e-cccc-1f3b5d6a7c8e"), new Guid("b3e1a8d2-4c5f-4b8e-9a2e-1f3b5d6a7c8e"), new Guid("41111111-4567-4567-4567-123456789aab") },
                    { new Guid("d03f54ec-1562-4c3f-a763-02da0fb8d2f3"), new Guid("45678901-4567-4567-4567-1234567890ab"), new Guid("34567890-3456-3456-3456-1234567890ab") },
                    { new Guid("d03f54ec-1562-4c3f-aaaa-02da0fb8d2f3"), new Guid("45678901-4567-4567-4567-1234567890ab"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("d03f54ec-1562-4c3f-cccc-02da0fb8d2f3"), new Guid("45678901-4567-4567-4567-1234567890ab"), new Guid("41111111-4567-4567-4567-123456789aab") },
                    { new Guid("d1e45413-f67f-46c5-aaaa-71881356c307"), new Guid("56789012-5678-5678-5678-1234567890ab"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("d1e45413-f67f-46c5-ac84-71881356c307"), new Guid("56789012-5678-5678-5678-1234567890ab"), new Guid("34567890-3456-3456-3456-1234567890ab") },
                    { new Guid("d1e45413-f67f-46c5-cccc-71881356c307"), new Guid("56789012-5678-5678-5678-1234567890ab"), new Guid("41111111-4567-4567-4567-123456789aab") },
                    { new Guid("d619e786-fd3e-47e4-8866-a091db79f523"), new Guid("67890123-6789-6789-6789-1234567890ab"), new Guid("34567890-3456-3456-3456-1234567890ab") },
                    { new Guid("d619e786-fd3e-47e4-aaaa-a091db79f523"), new Guid("67890123-6789-6789-6789-1234567890ab"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("d619e786-fd3e-47e4-cccc-a091db79f523"), new Guid("67890123-6789-6789-6789-1234567890ab"), new Guid("41111111-4567-4567-4567-123456789aab") },
                    { new Guid("da63d420-8bce-44dc-aaaa-da3f9e4bf1bc"), new Guid("78901234-7890-7890-7890-1234567890ab"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("da63d420-8bce-44dc-b96f-da3f9e4bf1bc"), new Guid("78901234-7890-7890-7890-1234567890ab"), new Guid("34567890-3456-3456-3456-1234567890ab") },
                    { new Guid("da63d420-8bce-44dc-cccc-da3f9e4bf1bc"), new Guid("78901234-7890-7890-7890-1234567890ab"), new Guid("41111111-4567-4567-4567-123456789aab") },
                    { new Guid("f7a3a3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b"), new Guid("01234567-0123-0123-0123-1234567890ab"), new Guid("23456789-2345-2345-2345-1234567890ab") },
                    { new Guid("f7b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b"), new Guid("90123456-9012-9012-9012-1234567890ab"), new Guid("12345678-1234-1234-1234-1234567890ab") },
                    { new Guid("fe965e49-84a3-4cc6-aaaa-a1ea2ed39562"), new Guid("89012345-8901-8901-8901-1234567890ab"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("fe965e49-84a3-4cc6-aed0-a1ea2ed39562"), new Guid("89012345-8901-8901-8901-1234567890ab"), new Guid("34567890-3456-3456-3456-1234567890ab") },
                    { new Guid("fe965e49-84a3-4cc6-cccc-a1ea2ed39562"), new Guid("89012345-8901-8901-8901-1234567890ab"), new Guid("41111111-4567-4567-4567-123456789aab") },
                    { new Guid("ffa3a3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b"), new Guid("12345678-1234-1234-1234-1234567aaaab"), new Guid("34567890-3456-3456-3456-1234567890ab") },
                    { new Guid("ffa3a3b3-3b3b-3b3b-aaaa-3b3b3b3b3b3b"), new Guid("12345678-1234-1234-1234-1234567aaaab"), new Guid("45678901-4567-4567-4567-123456789aab") },
                    { new Guid("ffa3a3b3-3b3b-3b3b-cccc-3b3b3b3b3b3b"), new Guid("12345678-1234-1234-1234-1234567aaaab"), new Guid("41111111-4567-4567-4567-123456789aab") }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HasCompany", "LastName", "Name", "Password", "ProfileImage", "RoleId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2024, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@gmail.com", false, "SuperAdmin", "SuperAdmin", "Super-Admin1", null, new Guid("12345678-1234-1234-1234-1234567890ab") });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_OwnerId",
                table: "Companies",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceImages_SmartDeviceId",
                table: "DeviceImages",
                column: "SmartDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeDevice_DeviceId",
                table: "HomeDevice",
                column: "DeviceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeDevice_HomeId",
                table: "HomeDevice",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeDevice_RoomId",
                table: "HomeDevice",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeMemberNotifications_HomeMemberId",
                table: "HomeMemberNotifications",
                column: "HomeMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeMemberNotifications_NotificationId",
                table: "HomeMemberNotifications",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeMembers_HomeId",
                table: "HomeMembers",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeMembers_UserId",
                table: "HomeMembers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Homes_OwnerId",
                table: "Homes",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberHomePermission_MemberId",
                table: "MemberHomePermission",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberHomePermission_PermissionId",
                table: "MemberHomePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_HomeDeviceId",
                table: "Notifications",
                column: "HomeDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleSystemPermission_PermissionId",
                table: "RoleSystemPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleSystemPermission_RoleId",
                table: "RoleSystemPermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HomeId",
                table: "Rooms",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartDevices_CompanyOwnerId",
                table: "SmartDevices",
                column: "CompanyOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceImages");

            migrationBuilder.DropTable(
                name: "HomeMemberNotifications");

            migrationBuilder.DropTable(
                name: "MemberHomePermission");

            migrationBuilder.DropTable(
                name: "RoleSystemPermission");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "HomeMembers");

            migrationBuilder.DropTable(
                name: "HomePermissions");

            migrationBuilder.DropTable(
                name: "SystemPermissions");

            migrationBuilder.DropTable(
                name: "HomeDevice");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "SmartDevices");

            migrationBuilder.DropTable(
                name: "Homes");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
