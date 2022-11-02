using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTracker.Server.Data.Migrations
{
    public partial class AddedGpsSnUniqueConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_GpsDevice_GpsDeviceId",
                table: "Pets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GpsDevice",
                table: "GpsDevice");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4601446c-81fe-4221-9734-533dc24345a9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a59398b4-64b3-4416-b5bb-a0281d345584");

            migrationBuilder.RenameTable(
                name: "GpsDevice",
                newName: "GpsDevices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GpsDevices",
                table: "GpsDevices",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "814473c5-7aec-4267-93be-6667e0edb2ed", "8964147b-48ca-4937-be5d-b2b246ad1001", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "90321224-4bf6-458c-816f-08d78ed7f3de", "61f08139-6ab1-4a73-a5d9-bedc07ed9c4b", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.CreateIndex(
                name: "IX_GpsDevices_SerialNumber",
                table: "GpsDevices",
                column: "SerialNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_GpsDevices_GpsDeviceId",
                table: "Pets",
                column: "GpsDeviceId",
                principalTable: "GpsDevices",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_GpsDevices_GpsDeviceId",
                table: "Pets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GpsDevices",
                table: "GpsDevices");

            migrationBuilder.DropIndex(
                name: "IX_GpsDevices_SerialNumber",
                table: "GpsDevices");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "814473c5-7aec-4267-93be-6667e0edb2ed");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90321224-4bf6-458c-816f-08d78ed7f3de");

            migrationBuilder.RenameTable(
                name: "GpsDevices",
                newName: "GpsDevice");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GpsDevice",
                table: "GpsDevice",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4601446c-81fe-4221-9734-533dc24345a9", "19deb977-14a5-49a0-bd8a-0d85297b9a77", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a59398b4-64b3-4416-b5bb-a0281d345584", "09b1326c-88ee-4451-aae4-d61b9caa1a1e", "User", "USER" });

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_GpsDevice_GpsDeviceId",
                table: "Pets",
                column: "GpsDeviceId",
                principalTable: "GpsDevice",
                principalColumn: "Id");
        }
    }
}
