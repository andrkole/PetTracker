using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTracker.Server.Data.Migrations
{
    public partial class OneToOneRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_AirInfoSensors_AirInfoSensorId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_FlameDetectionData_FlameDetectionDataId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_FlameDetectionSensors_FlameDetectionSensorId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_GpsDevices_GpsDeviceId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_AirInfoSensorId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_FlameDetectionDataId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_FlameDetectionSensorId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_GpsDeviceId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "AirInfoSensorId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "FlameDetectionDataId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "FlameDetectionSensorId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "GpsDeviceId",
                table: "Pets");

            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "GpsDevices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "FlameDetectionSensors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "FlameDetectionData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "AirInfoSensors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "AirInfoData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GpsDevices_PetId",
                table: "GpsDevices",
                column: "PetId",
                unique: true,
                filter: "[PetId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FlameDetectionSensors_PetId",
                table: "FlameDetectionSensors",
                column: "PetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlameDetectionData_PetId",
                table: "FlameDetectionData",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_AirInfoSensors_PetId",
                table: "AirInfoSensors",
                column: "PetId",
                unique: true,
                filter: "[PetId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AirInfoData_PetId",
                table: "AirInfoData",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_AirInfoData_Pets_PetId",
                table: "AirInfoData",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AirInfoSensors_Pets_PetId",
                table: "AirInfoSensors",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FlameDetectionData_Pets_PetId",
                table: "FlameDetectionData",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlameDetectionSensors_Pets_PetId",
                table: "FlameDetectionSensors",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_GpsDevices_Pets_PetId",
                table: "GpsDevices",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirInfoData_Pets_PetId",
                table: "AirInfoData");

            migrationBuilder.DropForeignKey(
                name: "FK_AirInfoSensors_Pets_PetId",
                table: "AirInfoSensors");

            migrationBuilder.DropForeignKey(
                name: "FK_FlameDetectionData_Pets_PetId",
                table: "FlameDetectionData");

            migrationBuilder.DropForeignKey(
                name: "FK_FlameDetectionSensors_Pets_PetId",
                table: "FlameDetectionSensors");

            migrationBuilder.DropForeignKey(
                name: "FK_GpsDevices_Pets_PetId",
                table: "GpsDevices");

            migrationBuilder.DropIndex(
                name: "IX_GpsDevices_PetId",
                table: "GpsDevices");

            migrationBuilder.DropIndex(
                name: "IX_FlameDetectionSensors_PetId",
                table: "FlameDetectionSensors");

            migrationBuilder.DropIndex(
                name: "IX_FlameDetectionData_PetId",
                table: "FlameDetectionData");

            migrationBuilder.DropIndex(
                name: "IX_AirInfoSensors_PetId",
                table: "AirInfoSensors");

            migrationBuilder.DropIndex(
                name: "IX_AirInfoData_PetId",
                table: "AirInfoData");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "GpsDevices");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "FlameDetectionSensors");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "FlameDetectionData");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "AirInfoSensors");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "AirInfoData");

            migrationBuilder.AddColumn<Guid>(
                name: "AirInfoSensorId",
                table: "Pets",
                type: "uniqueidentifier",
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
                table: "Pets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_AirInfoSensorId",
                table: "Pets",
                column: "AirInfoSensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_FlameDetectionDataId",
                table: "Pets",
                column: "FlameDetectionDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_FlameDetectionSensorId",
                table: "Pets",
                column: "FlameDetectionSensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_GpsDeviceId",
                table: "Pets",
                column: "GpsDeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_AirInfoSensors_AirInfoSensorId",
                table: "Pets",
                column: "AirInfoSensorId",
                principalTable: "AirInfoSensors",
                principalColumn: "Id");

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
                name: "FK_Pets_GpsDevices_GpsDeviceId",
                table: "Pets",
                column: "GpsDeviceId",
                principalTable: "GpsDevices",
                principalColumn: "Id");
        }
    }
}
