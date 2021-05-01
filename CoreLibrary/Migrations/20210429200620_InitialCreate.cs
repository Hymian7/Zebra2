using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zebra.Library.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Part",
                columns: table => new
                {
                    PartID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 35, nullable: false),
                    Position = table.Column<short>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdate = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Part", x => x.PartID);
                });

            migrationBuilder.CreateTable(
                name: "Piece",
                columns: table => new
                {
                    PieceID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 35, nullable: false),
                    Arranger = table.Column<string>(maxLength: 35, nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdate = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Tags = table.Column<string>(nullable: true),
                    Memo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Piece", x => x.PieceID);
                });

            migrationBuilder.CreateTable(
                name: "Setlist",
                columns: table => new
                {
                    SetlistID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreationDate = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdate = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setlist", x => x.SetlistID);
                });

            migrationBuilder.CreateTable(
                name: "Sheet",
                columns: table => new
                {
                    SheetID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreationDate = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdate = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Part_Id = table.Column<int>(nullable: true),
                    Piece_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sheet", x => x.SheetID);
                    table.ForeignKey(
                        name: "FK_Sheet_Part_Part_Id",
                        column: x => x.Part_Id,
                        principalTable: "Part",
                        principalColumn: "PartID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sheet_Piece_Piece_Id",
                        column: x => x.Piece_Id,
                        principalTable: "Piece",
                        principalColumn: "PieceID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SetlistItem",
                columns: table => new
                {
                    SetlistItemID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreationDate = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdate = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Piece_Id = table.Column<int>(nullable: true),
                    Setlist_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetlistItem", x => x.SetlistItemID);
                    table.ForeignKey(
                        name: "FK_SetlistItem_Piece_Piece_Id",
                        column: x => x.Piece_Id,
                        principalTable: "Piece",
                        principalColumn: "PieceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SetlistItem_Setlist_Setlist_Id",
                        column: x => x.Setlist_Id,
                        principalTable: "Setlist",
                        principalColumn: "SetlistID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SetlistItem_Piece_Id",
                table: "SetlistItem",
                column: "Piece_Id");

            migrationBuilder.CreateIndex(
                name: "IX_SetlistItem_Setlist_Id",
                table: "SetlistItem",
                column: "Setlist_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sheet_Part_Id",
                table: "Sheet",
                column: "Part_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sheet_Piece_Id",
                table: "Sheet",
                column: "Piece_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SetlistItem");

            migrationBuilder.DropTable(
                name: "Sheet");

            migrationBuilder.DropTable(
                name: "Setlist");

            migrationBuilder.DropTable(
                name: "Part");

            migrationBuilder.DropTable(
                name: "Piece");
        }
    }
}
