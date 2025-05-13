using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureDocumentAnonymizationSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewerFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReviewerFeedback",
                table: "Makaleler",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewerFeedback",
                table: "Makaleler");
        }
    }
}
