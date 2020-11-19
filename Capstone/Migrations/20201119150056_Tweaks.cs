using System;
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
                keyValue: "64fdb72d-7421-46cb-9e84-31cf75fbd8b0");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "AppointmentBlocks");

            migrationBuilder.AlterColumn<string>(
                name: "StartTime",
                table: "AppointmentBlocks",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a379bfb5-519c-42ad-a75b-fa8e17bf7d08", "90ade048-5b81-434f-885b-01777d5bfa75", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a379bfb5-519c-42ad-a75b-fa8e17bf7d08");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "AppointmentBlocks",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "AppointmentBlocks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "64fdb72d-7421-46cb-9e84-31cf75fbd8b0", "95fe3a53-242d-4a04-b85e-8d4a0b43194a", "Admin", "ADMIN" });
        }
    }
}
