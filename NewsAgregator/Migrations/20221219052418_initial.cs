using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VueProjectBack.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RSSUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "NewsItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SourceName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsItems_Sources_SourceName",
                        column: x => x.SourceName,
                        principalTable: "Sources",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Name", "RSSUrl" },
                values: new object[,]
                {
                    { "Лента", "https://lenta.ru/rss" },
                    { "Медуза", "https://meduza.io/rss/all" },
                    { "РИА новости", "https://ria.ru/export/rss2/archive/index.xml" },
                    { "Mail.ru", "https://news.mail.ru/rss/" },
                    { "RT", "https://russian.rt.com/rss" }
                });

            migrationBuilder.InsertData(
                table: "NewsItems",
                columns: new[] { "Id", "Category", "CreationDate", "Description", "SourceName", "Title", "Url" },
                values: new object[] { 1, "Unknown", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Mail.ru", "Unknown", "" });

            migrationBuilder.CreateIndex(
                name: "IX_NewsItems_SourceName",
                table: "NewsItems",
                column: "SourceName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsItems");

            migrationBuilder.DropTable(
                name: "Sources");
        }
    }
}
