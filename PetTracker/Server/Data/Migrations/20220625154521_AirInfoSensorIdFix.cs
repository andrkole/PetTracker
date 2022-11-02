using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTracker.Server.Data.Migrations
{
    public partial class AirInfoSensorIdFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "SensorId",
                table: "AirInfoData",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_AirInfoData_SensorId",
                table: "AirInfoData",
                column: "SensorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AirInfoData_AirInfoSensors_SensorId",
                table: "AirInfoData",
                column: "SensorId",
                principalTable: "AirInfoSensors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirInfoData_AirInfoSensors_SensorId",
                table: "AirInfoData");

            migrationBuilder.DropIndex(
                name: "IX_AirInfoData_SensorId",
                table: "AirInfoData");

            migrationBuilder.AlterColumn<string>(
                name: "SensorId",
                table: "AirInfoData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "AirInfoSensorId",
                table: "AirInfoData",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AirInfoData_AirInfoSensorId",
                table: "AirInfoData",
                column: "AirInfoSensorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AirInfoData_AirInfoSensors_AirInfoSensorId",
                table: "AirInfoData",
                column: "AirInfoSensorId",
                principalTable: "AirInfoSensors",
                principalColumn: "Id");
        }
    }
}
