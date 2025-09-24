using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPostAndFurniture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6f3abf51-3178-440f-9649-6f8a910fd2d8"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("89702e40-6c9f-4c5a-826a-03d6691259b3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aa1e9520-2e2e-4814-a928-1a8e9f9478ab"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e2d8aafd-fa14-43f1-aa4b-d5ce49d8d537"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ebde3f21-d615-4d78-9f1c-1e6b39c1c9d0"));

            migrationBuilder.CreateTable(
                name: "Furnitures",
                columns: table => new
                {
                    FurId = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Furnitures", x => x.FurId);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Condition = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DepositPolicy = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Posts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostFurnitures",
                columns: table => new
                {
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FurId = table.Column<string>(type: "nvarchar(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostFurnitures", x => new { x.PostId, x.FurId });
                    table.ForeignKey(
                        name: "FK_PostFurniture_Furniture_FurId",
                        column: x => x.FurId,
                        principalTable: "Furnitures",
                        principalColumn: "FurId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostFurniture_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("07e7d895-1a84-492d-8210-1b68a6440c65"), null, "PlusMember", "PLUSMEMBER" },
                    { new Guid("0d387877-be2a-4ddc-963b-cc4c55504988"), null, "Moderator", "MODERATOR" },
                    { new Guid("5926dc4d-aed7-4cc6-b9b5-725572ad01b1"), null, "ProMember", "PROMEMBER" },
                    { new Guid("6b7c6c65-108b-4ca0-b4fd-0ac664356637"), null, "Admin", "ADMIN" },
                    { new Guid("a00d9c3a-1e42-4cfd-9ef6-8c57c37d80d3"), null, "BasicMember", "BASICMEMBER" }
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Furnitu__1788CC4D8C1E3A2E",
                table: "Furnitures",
                column: "FurId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostFurnitures_FurId",
                table: "PostFurnitures",
                column: "FurId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostFurnitures");

            migrationBuilder.DropTable(
                name: "Furnitures");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("07e7d895-1a84-492d-8210-1b68a6440c65"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0d387877-be2a-4ddc-963b-cc4c55504988"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5926dc4d-aed7-4cc6-b9b5-725572ad01b1"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6b7c6c65-108b-4ca0-b4fd-0ac664356637"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a00d9c3a-1e42-4cfd-9ef6-8c57c37d80d3"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("6f3abf51-3178-440f-9649-6f8a910fd2d8"), null, "PlusMember", "PLUSMEMBER" },
                    { new Guid("89702e40-6c9f-4c5a-826a-03d6691259b3"), null, "ProMember", "PROMEMBER" },
                    { new Guid("aa1e9520-2e2e-4814-a928-1a8e9f9478ab"), null, "Moderator", "MODERATOR" },
                    { new Guid("e2d8aafd-fa14-43f1-aa4b-d5ce49d8d537"), null, "Admin", "ADMIN" },
                    { new Guid("ebde3f21-d615-4d78-9f1c-1e6b39c1c9d0"), null, "BasicMember", "BASICMEMBER" }
                });
        }
    }
}
