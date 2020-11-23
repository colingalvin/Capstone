using Microsoft.EntityFrameworkCore.Migrations;

namespace Capstone.Migrations
{
    public partial class Tweaks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cacb9364-e1d0-44d4-8870-82c71691c716");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Appointments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Appointments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3811f6ec-929b-4ef3-b7da-751f08f65bf0", "9bee316a-2ea1-4d2d-afc6-e12f3cb319c0", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3811f6ec-929b-4ef3-b7da-751f08f65bf0");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Appointments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cacb9364-e1d0-44d4-8870-82c71691c716", "2039ff13-1714-4484-a8a8-f21b6da10ee4", "Admin", "ADMIN" });
        }
    }
}
