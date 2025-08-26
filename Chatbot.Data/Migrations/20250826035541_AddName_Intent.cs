using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chatbot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddName_Intent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Intents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Intents");
        }
    }
}
