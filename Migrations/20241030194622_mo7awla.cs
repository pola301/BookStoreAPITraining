using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookInfo.API.Migrations
{
    /// <inheritdoc />
    public partial class mo7awla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "classOfInterests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    BookId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classOfInterests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_classOfInterests_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Ashton Kutcher", "Fucking shite", "48 laws of power" },
                    { 2, "Dean Bernat", "Fucking shite", "The Idiot Brain" },
                    { 3, "Gustave Le Bon", "Fucking shite", "Psychology of the Masses" }
                });

            migrationBuilder.InsertData(
                table: "classOfInterests",
                columns: new[] { "Id", "BookId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Fucking Shite", "Never Outshine the Master" },
                    { 2, 2, "Shitee", "Long Memory Short Memory" },
                    { 3, 1, "Fucking Shite", "Reputation Protect It" },
                    { 4, 2, "Shitee", "Alcohol and Memory" },
                    { 5, 3, "Fucking Shite", "The Masses are Easy to Manipulate" },
                    { 6, 3, "Shitee", "Mortada Mansour" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_classOfInterests_BookId",
                table: "classOfInterests",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "classOfInterests");

            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
