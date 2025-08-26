using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chatbot.Data.Migrations
{
    /// <inheritdoc />
    public partial class FK_KeywordBoost_Intent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeywordBoosts_Intents_IntentId",
                table: "KeywordBoosts");

            migrationBuilder.AlterColumn<int>(
                name: "IntentId",
                table: "KeywordBoosts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KeywordBoosts_Intents_IntentId",
                table: "KeywordBoosts",
                column: "IntentId",
                principalTable: "Intents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeywordBoosts_Intents_IntentId",
                table: "KeywordBoosts");

            migrationBuilder.AlterColumn<int>(
                name: "IntentId",
                table: "KeywordBoosts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_KeywordBoosts_Intents_IntentId",
                table: "KeywordBoosts",
                column: "IntentId",
                principalTable: "Intents",
                principalColumn: "Id");
        }
    }
}
