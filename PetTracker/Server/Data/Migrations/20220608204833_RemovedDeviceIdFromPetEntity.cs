using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTracker.Server.Data.Migrations
{
    public partial class RemovedDeviceIdFromPetEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "814473c5-7aec-4267-93be-6667e0edb2ed");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90321224-4bf6-458c-816f-08d78ed7f3de");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Pets");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a46379a7-6fe4-4573-884f-556dd426bda1", "e4bedd12-bc99-4112-a47c-ee14ba44c3c3", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c5181b12-29f5-4a18-a6e0-fb2c923167f9", "50f8700f-bfa7-4a7b-b223-b36d71b71038", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a46379a7-6fe4-4573-884f-556dd426bda1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c5181b12-29f5-4a18-a6e0-fb2c923167f9");

            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "814473c5-7aec-4267-93be-6667e0edb2ed", "8964147b-48ca-4937-be5d-b2b246ad1001", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "90321224-4bf6-458c-816f-08d78ed7f3de", "61f08139-6ab1-4a73-a5d9-bedc07ed9c4b", "Administrator", "ADMINISTRATOR" });
        }
    }
}
