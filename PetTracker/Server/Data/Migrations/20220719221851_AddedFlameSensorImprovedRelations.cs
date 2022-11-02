using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTracker.Server.Data.Migrations
{
    public partial class AddedFlameSensorImprovedRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetAreaCenterLocation_Pets_PetId",
                table: "PetAreaCenterLocation");

            migrationBuilder.DropIndex(
                name: "IX_PetAreaCenterLocation_PetId",
                table: "PetAreaCenterLocation");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "PetAreaCenterLocation");

            migrationBuilder.AddColumn<int>(
                name: "CenterLocationId",
                table: "Pets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlameDetectionDataId",
                table: "Pets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FlameDetectionSensorId",
                table: "Pets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GpsDeviceId",
                table: "PetLocations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "FlameDetectionSensors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlameDetectionSensors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlameDetectionData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlameDetected = table.Column<bool>(type: "bit", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SensorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlameDetectionData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlameDetectionData_FlameDetectionSensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "FlameDetectionSensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pets_CenterLocationId",
                table: "Pets",
                column: "CenterLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_FlameDetectionDataId",
                table: "Pets",
                column: "FlameDetectionDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_FlameDetectionSensorId",
                table: "Pets",
                column: "FlameDetectionSensorId");

            migrationBuilder.CreateIndex(
                name: "IX_PetLocations_GpsDeviceId",
                table: "PetLocations",
                column: "GpsDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_FlameDetectionData_SensorId",
                table: "FlameDetectionData",
                column: "SensorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PetLocations_GpsDevices_GpsDeviceId",
                table: "PetLocations",
                column: "GpsDeviceId",
                principalTable: "GpsDevices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_FlameDetectionData_FlameDetectionDataId",
                table: "Pets",
                column: "FlameDetectionDataId",
                principalTable: "FlameDetectionData",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_FlameDetectionSensors_FlameDetectionSensorId",
                table: "Pets",
                column: "FlameDetectionSensorId",
                principalTable: "FlameDetectionSensors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_PetAreaCenterLocation_CenterLocationId",
                table: "Pets",
                column: "CenterLocationId",
                principalTable: "PetAreaCenterLocation",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetLocations_GpsDevices_GpsDeviceId",
                table: "PetLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_FlameDetectionData_FlameDetectionDataId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_FlameDetectionSensors_FlameDetectionSensorId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_PetAreaCenterLocation_CenterLocationId",
                table: "Pets");

            migrationBuilder.DropTable(
                name: "FlameDetectionData");

            migrationBuilder.DropTable(
                name: "FlameDetectionSensors");

            migrationBuilder.DropIndex(
                name: "IX_Pets_CenterLocationId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_FlameDetectionDataId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_FlameDetectionSensorId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_PetLocations_GpsDeviceId",
                table: "PetLocations");

            migrationBuilder.DropColumn(
                name: "CenterLocationId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "FlameDetectionDataId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "FlameDetectionSensorId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "GpsDeviceId",
                table: "PetLocations");

            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "PetAreaCenterLocation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PetAreaCenterLocation_PetId",
                table: "PetAreaCenterLocation",
                column: "PetId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PetAreaCenterLocation_Pets_PetId",
                table: "PetAreaCenterLocation",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
