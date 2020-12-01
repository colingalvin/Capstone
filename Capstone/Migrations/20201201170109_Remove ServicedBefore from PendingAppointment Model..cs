using Microsoft.EntityFrameworkCore.Migrations;

namespace Capstone.Migrations
{
    public partial class RemoveServicedBeforefromPendingAppointmentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e19c6947-4506-49de-8d9b-138e07c803ca");

            migrationBuilder.DropColumn(
                name: "ServicedBefore",
                table: "PendingAppointments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "13d9eed7-6dfd-4a1b-8383-7c6469ecf3de", "21635da7-8f70-40f3-9ac9-604e3d450851", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13d9eed7-6dfd-4a1b-8383-7c6469ecf3de");

            migrationBuilder.AddColumn<bool>(
                name: "ServicedBefore",
                table: "PendingAppointments",
                type: "bit",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e19c6947-4506-49de-8d9b-138e07c803ca", "92c5c954-5ef9-44c6-85c1-131004f6df09", "Admin", "ADMIN" });
        }
    }
}
