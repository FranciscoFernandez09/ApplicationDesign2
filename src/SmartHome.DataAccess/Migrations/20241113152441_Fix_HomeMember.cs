using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Fix_HomeMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_HomeMembers_UserId_HomeId",
                table: "HomeMembers",
                columns: new[] { "UserId", "HomeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_HomeMembers_UserId_HomeId",
                table: "HomeMembers");

            migrationBuilder.CreateIndex(
                name: "IX_HomeMembers_UserId",
                table: "HomeMembers",
                column: "UserId",
                unique: true);
        }
    }
}
