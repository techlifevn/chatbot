using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chatbot.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Intents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultResponse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreateByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateOnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Synonyms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainTerm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SynonymText = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreateByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateOnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Synonyms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsStatus = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserInput = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BotResponse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Confidence = table.Column<float>(type: "real", nullable: false),
                    IntentId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreateByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateOnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationLogs_Intents_IntentId",
                        column: x => x.IntentId,
                        principalTable: "Intents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KeywordBoosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Keyword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Boost = table.Column<double>(type: "float", nullable: false),
                    IntentId = table.Column<int>(type: "int", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreateByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateOnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordBoosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeywordBoosts_Intents_IntentId",
                        column: x => x.IntentId,
                        principalTable: "Intents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Patterns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatternText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntentId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreateByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateOnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patterns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patterns_Intents_IntentId",
                        column: x => x.IntentId,
                        principalTable: "Intents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Responses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResponseText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsageCount = table.Column<int>(type: "int", nullable: false),
                    IntentId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreateByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateOnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responses_Intents_IntentId",
                        column: x => x.IntentId,
                        principalTable: "Intents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Embeddings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmbeddingData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IntentId = table.Column<int>(type: "int", nullable: false),
                    PatternId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Embeddings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Embeddings_Intents_IntentId",
                        column: x => x.IntentId,
                        principalTable: "Intents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Embeddings_Patterns_PatternId",
                        column: x => x.PatternId,
                        principalTable: "Patterns",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversationLogs_IntentId",
                table: "ConversationLogs",
                column: "IntentId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationLogs_SessionId",
                table: "ConversationLogs",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Embeddings_IntentId",
                table: "Embeddings",
                column: "IntentId");

            migrationBuilder.CreateIndex(
                name: "IX_Embeddings_PatternId",
                table: "Embeddings",
                column: "PatternId");

            migrationBuilder.CreateIndex(
                name: "IX_KeywordBoosts_IntentId",
                table: "KeywordBoosts",
                column: "IntentId");

            migrationBuilder.CreateIndex(
                name: "IX_Patterns_IntentId",
                table: "Patterns",
                column: "IntentId");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_IntentId",
                table: "Responses",
                column: "IntentId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Synonyms_SynonymText",
                table: "Synonyms",
                column: "SynonymText");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversationLogs");

            migrationBuilder.DropTable(
                name: "Embeddings");

            migrationBuilder.DropTable(
                name: "KeywordBoosts");

            migrationBuilder.DropTable(
                name: "Responses");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Synonyms");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Patterns");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Intents");
        }
    }
}
