using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTracker.Server.Data.Migrations
{
    public partial class AddedAirInfoSensorAndPetLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AirInfoSensorId",
                table: "Pets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AirInfoSensors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirInfoSensors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PetLocations",
                columns: table => new
                {
                    PetLocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetLocations", x => x.PetLocationId);
                    table.ForeignKey(
                        name: "FK_PetLocations_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pets_AirInfoSensorId",
                table: "Pets",
                column: "AirInfoSensorId");

            migrationBuilder.CreateIndex(
                name: "IX_AirInfoSensors_SerialNumber",
                table: "AirInfoSensors",
                column: "SerialNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PetLocations_PetId",
                table: "PetLocations",
                column: "PetId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_AirInfoSensors_AirInfoSensorId",
                table: "Pets",
                column: "AirInfoSensorId",
                principalTable: "AirInfoSensors",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_AirInfoSensors_AirInfoSensorId",
                table: "Pets");

            migrationBuilder.DropTable(
                name: "AirInfoSensors");

            migrationBuilder.DropTable(
                name: "PetLocations");

            migrationBuilder.DropIndex(
                name: "IX_Pets_AirInfoSensorId",
                table: "Pets");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "05800faa-1162-4b9c-ab26-53300c623b00");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9981e26-0e4a-4662-a366-784b74757e3c");

            migrationBuilder.DropColumn(
                name: "AirInfoSensorId",
                table: "Pets");
        }
    }
}
