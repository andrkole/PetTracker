using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTracker.Server.Data.Migrations
{
    public partial class AddedGpsDeviceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "651412a5-183c-4b1a-bbcb-dd0d7bac41f9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "94815d91-f2ac-43f2-8f61-095727265d11");

            migrationBuilder.RenameColumn(
                name: "GpsId",
                table: "Pets",
                newName: "DeviceId");

            migrationBuilder.AddColumn<Guid>(
                name: "GpsDeviceId",
                table: "Pets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GpsDevice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GpsDevice", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4601446c-81fe-4221-9734-533dc24345a9", "19deb977-14a5-49a0-bd8a-0d85297b9a77", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a59398b4-64b3-4416-b5bb-a0281d345584", "09b1326c-88ee-4451-aae4-d61b9caa1a1e", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_Pets_GpsDeviceId",
                table: "Pets",
                column: "GpsDeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_GpsDevice_GpsDeviceId",
                table: "Pets",
                column: "GpsDeviceId",
                principalTable: "GpsDevice",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_GpsDevice_GpsDeviceId",
                table: "Pets");

            migrationBuilder.DropTable(
                name: "GpsDevice");

            migrationBuilder.DropIndex(
                name: "IX_Pets_GpsDeviceId",
                table: "Pets");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4601446c-81fe-4221-9734-533dc24345a9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a59398b4-64b3-4416-b5bb-a0281d345584");

            migrationBuilder.DropColumn(
                name: "GpsDeviceId",
                table: "Pets");

            migrationBuilder.RenameColumn(
                name: "DeviceId",
                table: "Pets",
                newName: "GpsId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "651412a5-183c-4b1a-bbcb-dd0d7bac41f9", "09fb3431-52f5-4b1c-b3ba-3890d310d78e", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "94815d91-f2ac-43f2-8f61-095727265d11", "5808af69-239f-4120-b178-be2bd55c7be3", "Administrator", "ADMINISTRATOR" });
        }
    }
}
