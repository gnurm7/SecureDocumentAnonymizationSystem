using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureDocumentAnonymizationSystem.Migrations
{
    /// <inheritdoc />
    public partial class anonymizedfilename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnonymizedFileName",
                table: "Makaleler",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnonymizedFileName",
                table: "Makaleler");
        }
    }
}
