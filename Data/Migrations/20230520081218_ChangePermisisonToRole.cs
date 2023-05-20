using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResellHub.Migrations
{
    /// <inheritdoc />
    public partial class ChangePermisisonToRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Permissions",
                table: "Roles",
                newName: "UserRole");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserRole",
                table: "Roles",
                newName: "Permissions");
        }
    }
}
