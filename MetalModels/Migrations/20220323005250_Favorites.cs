using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetalModels.Migrations
{
    /// <inheritdoc />
    public partial class Favorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MacId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Smartphone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AndroidVersion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BandFavorites",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandFavorites", x => new { x.UserId, x.BandId });
                    table.ForeignKey(
                        name: "FK_BandFavorites_Bands_UserId",
                        column: x => x.UserId,
                        principalTable: "Bands",
                        principalColumn: "BandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandFavorites_User_BandId",
                        column: x => x.BandId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenreFavorites",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreFavorites", x => new { x.UserId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_GenreFavorites_Genres_UserId",
                        column: x => x.UserId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreFavorites_User_GenreId",
                        column: x => x.GenreId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BandFavorites_BandId",
                table: "BandFavorites",
                column: "BandId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreFavorites_GenreId",
                table: "GenreFavorites",
                column: "GenreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BandFavorites");

            migrationBuilder.DropTable(
                name: "GenreFavorites");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
