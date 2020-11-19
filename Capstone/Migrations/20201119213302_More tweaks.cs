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
                keyValue: "a379bfb5-519c-42ad-a75b-fa8e17bf7d08");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "IncludedServices",
                table: "Appointments",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3611a620-521a-48ef-ad08-b86c2cf2bb74", "5def0275-d47c-46e1-a6b0-9a64d51f30c7", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3611a620-521a-48ef-ad08-b86c2cf2bb74");

            migrationBuilder.DropColumn(
                name: "IncludedServices",
                table: "Appointments");

            migrationBuilder.AddColumn<bool>(
                name: "ApprovalStatus",
                table: "Appointments",
                type: "bit",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a379bfb5-519c-42ad-a75b-fa8e17bf7d08", "90ade048-5b81-434f-885b-01777d5bfa75", "Admin", "ADMIN" });
        }
    }
}
