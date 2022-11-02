using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTracker.Server.Data.Migrations
{
    public partial class OneToManyAirInfoSensor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PetLocationId",
                table: "PetLocations",
                newName: "Id");

            migrationBuilder.CreateTable(
                name: "AirInfoData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirTemperature = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AirHumidity = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SensorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AirInfoSensorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirInfoData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirInfoData_AirInfoSensors_AirInfoSensorId",
                        column: x => x.AirInfoSensorId,
                        principalTable: "AirInfoSensors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirInfoData_AirInfoSensorId",
                table: "AirInfoData",
                column: "AirInfoSensorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirInfoData");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PetLocations",
                newName: "PetLocationId");
        }
    }
}
