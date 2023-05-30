using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResellHub.Migrations
{
    /// <inheritdoc />
    public partial class Add3ImagesToOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image1",
                table: "Offers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image2",
                table: "Offers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image3",
                table: "Offers",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image1",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Image2",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Image3",
                table: "Offers");
        }
    }
}
