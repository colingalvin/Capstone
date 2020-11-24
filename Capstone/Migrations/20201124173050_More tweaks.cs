using Microsoft.EntityFrameworkCore.Migrations;

namespace Capstone.Migrations
{
    public partial class Moretweaks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3811f6ec-929b-4ef3-b7da-751f08f65bf0");

            migrationBuilder.DropColumn(
                name: "IncludedServices",
                table: "PendingAppointments");

            migrationBuilder.DropColumn(
                name: "TechnicianNotes",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IncludedServices",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "Services",
                table: "PendingAppointments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Services",
                table: "Appointments",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a01ad17a-f973-42f1-acae-001d8ade806c", "057624f7-1426-4963-bc2c-f9273d6ac45f", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a01ad17a-f973-42f1-acae-001d8ade806c");

            migrationBuilder.DropColumn(
                name: "Services",
                table: "PendingAppointments");

            migrationBuilder.DropColumn(
                name: "Services",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "IncludedServices",
                table: "PendingAppointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TechnicianNotes",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncludedServices",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3811f6ec-929b-4ef3-b7da-751f08f65bf0", "9bee316a-2ea1-4d2d-afc6-e12f3cb319c0", "Admin", "ADMIN" });
        }
    }
}
