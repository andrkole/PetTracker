using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTracker.Server.Data.Migrations
{
    public partial class OneToManyRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetLocations_Pets_PetId",
                table: "PetLocations");

            migrationBuilder.DropIndex(
                name: "IX_PetLocations_PetId",
                table: "PetLocations");

            migrationBuilder.AlterColumn<int>(
                name: "PetId",
                table: "PetLocations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PetLocations_PetId",
                table: "PetLocations",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_PetLocations_Pets_PetId",
                table: "PetLocations",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetLocations_Pets_PetId",
                table: "PetLocations");

            migrationBuilder.DropIndex(
                name: "IX_PetLocations_PetId",
                table: "PetLocations");

            migrationBuilder.AlterColumn<int>(
                name: "PetId",
                table: "PetLocations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_PetLocations_PetId",
                table: "PetLocations",
                column: "PetId",
                unique: true,
                filter: "[PetId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PetLocations_Pets_PetId",
                table: "PetLocations",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id");
        }
    }
}
