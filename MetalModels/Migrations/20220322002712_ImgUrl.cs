using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetalModels.Migrations
{
    /// <inheritdoc />
    public partial class ImgUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReleaseYear",
                table: "Albums",
                newName: "ReleaseDate");

            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Bands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Albums",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Bands");

            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Albums");

            migrationBuilder.RenameColumn(
                name: "ReleaseDate",
                table: "Albums",
                newName: "ReleaseYear");
        }
    }
}
