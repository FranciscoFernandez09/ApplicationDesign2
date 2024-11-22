using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartHome.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Add_home_permissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SystemPermissions",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("d2f1e8a4-3b6e-4b8e-9a2e-1f3b5d6a7c8e"), "GetHomes" });

            migrationBuilder.InsertData(
                table: "RoleSystemPermission",
                columns: new[] { "Id", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-7890-cccc-ef1234567890"), new Guid("d2f1e8a4-3b6e-4b8e-9a2e-1f3b5d6a7c8e"), new Guid("41111111-4567-4567-4567-123456789aab") },
                    { new Guid("e3b0c442-98fc-1c14-9ddf-4b6e6e6e6e6e"), new Guid("d2f1e8a4-3b6e-4b8e-9a2e-1f3b5d6a7c8e"), new Guid("34567890-3456-3456-3456-1234567890ab") },
                    { new Guid("f47ac10b-58cc-4372-aaaa-0e02b2c3d479"), new Guid("d2f1e8a4-3b6e-4b8e-9a2e-1f3b5d6a7c8e"), new Guid("45678901-4567-4567-4567-123456789aab") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-cccc-ef1234567890"));

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumn: "Id",
                keyValue: new Guid("e3b0c442-98fc-1c14-9ddf-4b6e6e6e6e6e"));

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumn: "Id",
                keyValue: new Guid("f47ac10b-58cc-4372-aaaa-0e02b2c3d479"));

            migrationBuilder.DeleteData(
                table: "SystemPermissions",
                keyColumn: "Id",
                keyValue: new Guid("d2f1e8a4-3b6e-4b8e-9a2e-1f3b5d6a7c8e"));
        }
    }
}
