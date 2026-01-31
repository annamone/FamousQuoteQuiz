using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamousQuoteQuiz.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGameModeToGameSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameMode",
                table: "GameSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameMode",
                table: "GameSessions");
        }
    }
}
