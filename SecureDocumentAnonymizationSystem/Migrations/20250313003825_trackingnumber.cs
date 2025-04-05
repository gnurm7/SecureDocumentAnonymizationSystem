using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureDocumentAnonymizationSystem.Migrations
{
    /// <inheritdoc />
    public partial class trackingnumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrackingNumber",
                table: "Makaleler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackingNumber",
                table: "Makaleler");
        }
    }
}
