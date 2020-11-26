using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pikaball.Migrations
{
    public partial class PokemonCollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PokemonCollections",
                columns: table => new
                {
                    PokedexID = table.Column<int>(nullable: false),
                    UserID = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    level = table.Column<int>(nullable: false),
                    LastDrawn = table.Column<DateTime>(nullable: false),
                    HasNextEvolution = table.Column<bool>(nullable: false),
                    EvCondition = table.Column<int>(nullable: true),
                    EvolutionUnlocked = table.Column<bool>(nullable: true),
                    SpriteUrl = table.Column<string>(nullable: true),
                    Type1 = table.Column<int>(nullable: false),
                    Type2 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonCollections", x => new { x.PokedexID, x.UserID });
                    table.ForeignKey(
                        name: "FK_PokemonCollections_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PokemonCollections_UserID",
                table: "PokemonCollections",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PokemonCollections");
        }
    }
}
