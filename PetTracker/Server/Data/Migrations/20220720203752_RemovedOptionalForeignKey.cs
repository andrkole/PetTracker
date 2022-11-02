using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTracker.Server.Data.Migrations
{
    public partial class RemovedOptionalForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirInfoSensors_Pets_PetId",
                table: "AirInfoSensors");

            migrationBuilder.DropForeignKey(
                name: "FK_AirInfoData_Pets_PetId",
                table: "AirInfoData");

            migrationBuilder.DropForeignKey(
                name: "FK_GpsDevices_Pets_PetId",
                table: "GpsDevices");

            migrationBuilder.DropForeignKey(
                name: "FK_PetLocations_Pets_PetId",
                table: "PetLocations");

            migrationBuilder.DropIndex(
                name: "IX_GpsDevices_PetId",
                table: "GpsDevices");

            migrationBuilder.DropIndex(
                name: "IX_AirInfoSensors_PetId",
                table: "AirInfoSensors");

            migrationBuilder.AlterColumn<int>(
                name: "PetId",
                table: "GpsDevices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PetId",
                table: "AirInfoSensors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GpsDevices_PetId",
                table: "GpsDevices",
                column: "PetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AirInfoSensors_PetId",
                table: "AirInfoSensors",
                column: "PetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AirInfoData_PetId",
                table: "AirInfoData",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_AirInfoSensors_Pets_PetId",
                table: "AirInfoSensors",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AirInfoData_Pets_PetId",
                table: "AirInfoData",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
               name: "FK_PetLocations_Pets_PetId",
               table: "PetLocations",
               column: "PetId",
               principalTable: "Pets",
               principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GpsDevices_Pets_PetId",
                table: "GpsDevices",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirInfoSensors_Pets_PetId",
                table: "AirInfoSensors");

            migrationBuilder.DropForeignKey(
                name: "FK_GpsDevices_Pets_PetId",
                table: "GpsDevices");

            migrationBuilder.DropIndex(
                name: "IX_GpsDevices_PetId",
                table: "GpsDevices");

            migrationBuilder.DropIndex(
                name: "IX_AirInfoSensors_PetId",
                table: "AirInfoSensors");

            migrationBuilder.AlterColumn<int>(
                name: "PetId",
                table: "GpsDevices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PetId",
                table: "AirInfoSensors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_GpsDevices_PetId",
                table: "GpsDevices",
                column: "PetId",
                unique: true,
                filter: "[PetId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AirInfoSensors_PetId",
                table: "AirInfoSensors",
                column: "PetId",
                unique: true,
                filter: "[PetId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AirInfoSensors_Pets_PetId",
                table: "AirInfoSensors",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GpsDevices_Pets_PetId",
                table: "GpsDevices",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id");
        }
    }
}
