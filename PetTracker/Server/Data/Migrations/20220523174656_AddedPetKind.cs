using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTracker.Server.Data.Migrations
{
    public partial class AddedPetKind : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Kind",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kind",
                table: "Pets");
        }
    }
}
