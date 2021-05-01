using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zebra.Library.Migrations
{
    public partial class AddSetlistNameAndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Setlist",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Setlist",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Setlist",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Setlist");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Setlist");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Setlist");
        }
    }
}
