using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTracker.Server.Data.Migrations
{
    public partial class AddedMovement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_PetAreaCenterLocation_CenterLocationId",
                table: "Pets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PetAreaCenterLocation",
                table: "PetAreaCenterLocation");

            migrationBuilder.RenameTable(
                name: "PetAreaCenterLocation",
                newName: "PetAreaCenterLocations");

            migrationBuilder.AddColumn<int>(
                name: "ActiveTimeSeconds",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RestingTimeSeconds",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PetAreaCenterLocations",
                table: "PetAreaCenterLocations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PetMovement",
                columns: table => new
                {
                    PetId = table.Column<int>(type: "int", nullable: false),
                    MovementSeconds = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_PetAreaCenterLocations_CenterLocationId",
                table: "Pets",
                column: "CenterLocationId",
                principalTable: "PetAreaCenterLocations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_PetAreaCenterLocations_CenterLocationId",
                table: "Pets");

            migrationBuilder.DropTable(
                name: "PetMovement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PetAreaCenterLocations",
                table: "PetAreaCenterLocations");

            migrationBuilder.DropColumn(
                name: "ActiveTimeSeconds",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "RestingTimeSeconds",
                table: "Pets");

            migrationBuilder.RenameTable(
                name: "PetAreaCenterLocations",
                newName: "PetAreaCenterLocation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PetAreaCenterLocation",
                table: "PetAreaCenterLocation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_PetAreaCenterLocation_CenterLocationId",
                table: "Pets",
                column: "CenterLocationId",
                principalTable: "PetAreaCenterLocation",
                principalColumn: "Id");
        }
    }
}
