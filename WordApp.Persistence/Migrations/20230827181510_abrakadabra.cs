using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WordApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class abrakadabra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_wordSets_levels_LevelId",
                table: "wordSets");

            migrationBuilder.DropTable(
                name: "levels");

            migrationBuilder.DropIndex(
                name: "IX_wordSets_LevelId",
                table: "wordSets");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "wordSets");

            migrationBuilder.AddColumn<byte>(
                name: "Level",
                table: "wordSets",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "wordSets");

            migrationBuilder.AddColumn<string>(
                name: "LevelId",
                table: "wordSets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "levels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Language = table.Column<byte>(type: "smallint", nullable: false),
                    LanguageId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_levels", x => x.Id);
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
                name: "IX_wordSets_LevelId",
                table: "wordSets",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_wordSets_levels_LevelId",
                table: "wordSets",
                column: "LevelId",
                principalTable: "levels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
