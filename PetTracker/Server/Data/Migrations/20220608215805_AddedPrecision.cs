using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTracker.Server.Data.Migrations
{
    public partial class AddedPrecision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a46379a7-6fe4-4573-884f-556dd426bda1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c5181b12-29f5-4a18-a6e0-fb2c923167f9");

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "Pets",
                type: "float(3)",
                precision: 3,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Age",
                table: "Pets",
                type: "float(2)",
                precision: 2,
                scale: 1,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7e266fe7-d208-412a-8a6a-eb4d63a07fa5", "05bcf27e-9ab2-4af5-9264-f490be9eec87", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9d1895fe-9778-4066-9a09-5b421fdc2e59", "be63fd6a-dc7a-45c8-8815-b20ed7414523", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7e266fe7-d208-412a-8a6a-eb4d63a07fa5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9d1895fe-9778-4066-9a09-5b421fdc2e59");

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "Pets",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float(3)",
                oldPrecision: 3,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "Pets",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float(2)",
                oldPrecision: 2,
                oldScale: 1,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a46379a7-6fe4-4573-884f-556dd426bda1", "e4bedd12-bc99-4112-a47c-ee14ba44c3c3", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c5181b12-29f5-4a18-a6e0-fb2c923167f9", "50f8700f-bfa7-4a7b-b223-b36d71b71038", "Administrator", "ADMINISTRATOR" });
        }
    }
}
