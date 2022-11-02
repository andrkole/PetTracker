using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTracker.Server.Data.Migrations
{
    public partial class AddedLastSync : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastSynchronization",
                table: "Pets",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSynchronization",
                table: "Pets");
        }
    }
}
