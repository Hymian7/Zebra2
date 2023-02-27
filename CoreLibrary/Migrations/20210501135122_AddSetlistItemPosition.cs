using Microsoft.EntityFrameworkCore.Migrations;

namespace Zebra.Library.Migrations
{
    public partial class AddSetlistItemPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "SetlistItem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "SetlistItem");
        }
    }
}
