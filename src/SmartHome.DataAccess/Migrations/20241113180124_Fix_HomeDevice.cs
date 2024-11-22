using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Fix_HomeDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HomeDevice_DeviceId",
                table: "HomeDevice");

            migrationBuilder.DropIndex(
                name: "IX_HomeDevice_HomeId",
                table: "HomeDevice");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_HomeDevice_HomeId_DeviceId",
                table: "HomeDevice",
                columns: new[] { "HomeId", "DeviceId" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeDevice_DeviceId",
                table: "HomeDevice",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_HomeDevice_HomeId_DeviceId",
                table: "HomeDevice");

            migrationBuilder.DropIndex(
                name: "IX_HomeDevice_DeviceId",
                table: "HomeDevice");

            migrationBuilder.CreateIndex(
                name: "IX_HomeDevice_DeviceId",
                table: "HomeDevice",
                column: "DeviceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeDevice_HomeId",
                table: "HomeDevice",
                column: "HomeId");
        }
    }
}
