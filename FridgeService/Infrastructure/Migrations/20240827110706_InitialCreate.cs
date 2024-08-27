using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    login = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    isAdmin = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "fridges",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    model = table.Column<string>(type: "text", nullable: false),
                    serial = table.Column<string>(type: "text", nullable: false),
                    boughtDate = table.Column<int>(type: "integer", nullable: false),
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
                    productId = table.Column<int>(type: "integer", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false),
                    fridgeId = table.Column<int>(type: "integer", nullable: false)
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
                        name: "FK_userFridges_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_userFridges_userId",
                table: "userFridges",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "productFridgeModels");

            migrationBuilder.DropTable(
                name: "userFridges");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "fridges");
        }
    }
}
