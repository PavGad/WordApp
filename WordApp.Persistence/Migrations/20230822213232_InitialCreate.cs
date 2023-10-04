using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WordApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "complaintReasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_complaintReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "levels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    LanguageId = table.Column<string>(type: "text", nullable: false),
                    Language = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_levels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "refreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpiresOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_refreshTokens_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userWords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetWord = table.Column<string>(type: "text", nullable: false),
                    OriginalWord = table.Column<string>(type: "text", nullable: false),
                    TargetContext = table.Column<string>(type: "text", nullable: false),
                    OriginalContext = table.Column<string>(type: "text", nullable: false),
                    Definition = table.Column<string>(type: "text", nullable: false),
                    RepeatOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Theme = table.Column<string>(type: "text", nullable: false),
                    Stage = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalLanguage = table.Column<byte>(type: "smallint", nullable: false),
                    TargetLanguage = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userWords_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "words",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetWord = table.Column<string>(type: "text", nullable: false),
                    OriginalWord = table.Column<string>(type: "text", nullable: false),
                    TargetContext = table.Column<string>(type: "text", nullable: false),
                    OriginalContext = table.Column<string>(type: "text", nullable: false),
                    Definition = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalLanguage = table.Column<byte>(type: "smallint", nullable: false),
                    TargetLanguage = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_words", x => x.Id);
                    table.ForeignKey(
                        name: "FK_words_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "wordSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    ConfirmedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CoverImageUrl = table.Column<string>(type: "text", nullable: false),
                    LevelId = table.Column<string>(type: "text", nullable: false),
                    ConfirmedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalLanguage = table.Column<byte>(type: "smallint", nullable: false),
                    TargetLanguage = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wordSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_wordSets_levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_wordSets_users_ConfirmedById",
                        column: x => x.ConfirmedById,
                        principalTable: "users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_wordSets_users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "complaints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReasonId = table.Column<int>(type: "integer", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    WordSetId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_complaints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_complaints_complaintReasons_ReasonId",
                        column: x => x.ReasonId,
                        principalTable: "complaintReasons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_complaints_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_complaints_wordSets_WordSetId",
                        column: x => x.WordSetId,
                        principalTable: "wordSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "proposedWords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetWord = table.Column<string>(type: "text", nullable: false),
                    OriginalWord = table.Column<string>(type: "text", nullable: false),
                    TargetContext = table.Column<string>(type: "text", nullable: false),
                    OriginalContext = table.Column<string>(type: "text", nullable: false),
                    Definition = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    WordSetId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proposedWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_proposedWords_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_proposedWords_wordSets_WordSetId",
                        column: x => x.WordSetId,
                        principalTable: "wordSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordWordSet",
                columns: table => new
                {
                    WordSetsId = table.Column<Guid>(type: "uuid", nullable: false),
                    WordsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordWordSet", x => new { x.WordSetsId, x.WordsId });
                    table.ForeignKey(
                        name: "FK_WordWordSet_wordSets_WordSetsId",
                        column: x => x.WordSetsId,
                        principalTable: "wordSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordWordSet_words_WordsId",
                        column: x => x.WordsId,
                        principalTable: "words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "complaintReasons",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "unacceptable content", "Unacceptable content" },
                    { 2, "Content mistakes", "Content mistakes" }
                });

            migrationBuilder.InsertData(
                table: "levels",
                columns: new[] { "Id", "Description", "Language", "LanguageId", "Name" },
                values: new object[,]
                {
                    { "A1", "CEFR", (byte)0, "eng", "Beginner" },
                    { "A2", "CEFR", (byte)0, "eng", "Elementary" },
                    { "B1", "CEFR", (byte)0, "eng", "Intermediate" },
                    { "B2", "CEFR", (byte)0, "eng", "Upper-Intermediate" },
                    { "C1", "CEFR", (byte)0, "eng", "Advanced" },
                    { "C2", "CEFR", (byte)0, "eng", "Proficiency" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_complaints_ReasonId",
                table: "complaints",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_complaints_UserId",
                table: "complaints",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_complaints_WordSetId",
                table: "complaints",
                column: "WordSetId");

            migrationBuilder.CreateIndex(
                name: "IX_proposedWords_UserId",
                table: "proposedWords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_proposedWords_WordSetId",
                table: "proposedWords",
                column: "WordSetId");

            migrationBuilder.CreateIndex(
                name: "IX_refreshTokens_UserId",
                table: "refreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_userWords_UserId",
                table: "userWords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_words_UserId",
                table: "words",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_wordSets_ConfirmedById",
                table: "wordSets",
                column: "ConfirmedById");

            migrationBuilder.CreateIndex(
                name: "IX_wordSets_CreatedById",
                table: "wordSets",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_wordSets_LevelId",
                table: "wordSets",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_WordWordSet_WordsId",
                table: "WordWordSet",
                column: "WordsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "complaints");

            migrationBuilder.DropTable(
                name: "proposedWords");

            migrationBuilder.DropTable(
                name: "refreshTokens");

            migrationBuilder.DropTable(
                name: "userWords");

            migrationBuilder.DropTable(
                name: "WordWordSet");

            migrationBuilder.DropTable(
                name: "complaintReasons");

            migrationBuilder.DropTable(
                name: "wordSets");

            migrationBuilder.DropTable(
                name: "words");

            migrationBuilder.DropTable(
                name: "levels");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
