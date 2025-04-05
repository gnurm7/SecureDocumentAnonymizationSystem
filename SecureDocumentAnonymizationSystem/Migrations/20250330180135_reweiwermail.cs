using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureDocumentAnonymizationSystem.Migrations
{
    /// <inheritdoc />
    public partial class reweiwermail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReviewerEmail",
                table: "Makaleler",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewerEmail",
                table: "Makaleler");
        }
    }
}
