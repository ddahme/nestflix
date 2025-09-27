using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class Geometry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "Point",
                table: "Boxes",
                type: "geography (Point,4326)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry");

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_Point",
                table: "Boxes",
                column: "Point")
                .Annotation("Npgsql:IndexMethod", "GIST");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Boxes_Point",
                table: "Boxes");

            migrationBuilder.AlterColumn<Point>(
                name: "Point",
                table: "Boxes",
                type: "geometry",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geography (Point,4326)");
        }
    }
}
