using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Fix_HomeMember_Finally : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_HomeMembers_UserId_HomeId",
                table: "HomeMembers");
            
            migrationBuilder.DropIndex(
                name: "IX_HomeMembers_UserId",
                table: "HomeMembers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_HomeMembers_UserId_HomeId",
                table: "HomeMembers",
                columns: new[] { "UserId", "HomeId" });
        }
    }
}
