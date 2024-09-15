using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "fridges",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    model = table.Column<string>(type: "text", nullable: false),
                    serial = table.Column<string>(type: "text", nullable: true),
                    boughtDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    boxNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fridges", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "productFridgeModels",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productId = table.Column<string>(type: "text", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false),
                    fridgeId = table.Column<int>(type: "integer", nullable: false),
                    addTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productFridgeModels", x => x.id);
                    table.ForeignKey(
                        name: "FK_productFridgeModels_fridges_fridgeId",
                        column: x => x.fridgeId,
                        principalTable: "fridges",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userFridges",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    fridgeId = table.Column<int>(type: "integer", nullable: false),
                    LinkTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userFridges", x => x.id);
                    table.ForeignKey(
                        name: "FK_userFridges_fridges_fridgeId",
                        column: x => x.fridgeId,
                        principalTable: "fridges",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_productFridgeModels_fridgeId",
                table: "productFridgeModels",
                column: "fridgeId");

            migrationBuilder.CreateIndex(
                name: "IX_userFridges_fridgeId",
                table: "userFridges",
                column: "fridgeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "productFridgeModels");

            migrationBuilder.DropTable(
                name: "userFridges");

            migrationBuilder.DropTable(
                name: "fridges");
        }
    }
}
