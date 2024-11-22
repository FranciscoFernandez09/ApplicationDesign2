using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Add_AK_HomeMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HomeMembers_HomeId",
                table: "HomeMembers");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_HomeMembers_HomeId_UserId",
                table: "HomeMembers",
                columns: new[] { "HomeId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_HomeMembers_HomeId_UserId",
                table: "HomeMembers");

            migrationBuilder.CreateIndex(
                name: "IX_HomeMembers_HomeId",
                table: "HomeMembers",
                column: "HomeId");
        }
    }
}
