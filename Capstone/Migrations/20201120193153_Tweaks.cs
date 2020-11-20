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
                keyValue: "43564cf0-7e62-4bf2-87a8-bb1d57aa0680");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "PendingAppointments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "PendingAppointments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "Day",
                table: "AppointmentBlocks",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "dbe5bbe6-6ffc-41e5-a384-009036f876c6", "2911a8ff-fc58-4098-b7ae-9d8a7eea1a5d", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dbe5bbe6-6ffc-41e5-a384-009036f876c6");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "PendingAppointments");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "PendingAppointments");

            migrationBuilder.AlterColumn<string>(
                name: "Day",
                table: "AppointmentBlocks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "43564cf0-7e62-4bf2-87a8-bb1d57aa0680", "773220a5-1114-46b3-8aa5-59272ab4ea64", "Admin", "ADMIN" });
        }
    }
}
