using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSubcription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("33a2f609-396a-4fa0-aaf6-8b2256767496"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9448c7d6-93ff-4bbf-9352-636bb411889a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a412f71d-40c7-4bbc-b267-1b01e4fe5f4c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a462a7d5-f20b-4e23-92b8-f65866a8a2cc"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b0887849-c0f1-472e-8cb8-c8c9ee758e5a"));

            migrationBuilder.CreateTable(
                name: "Subcriptions",
                columns: table => new
                {
                    SubcriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DurationInDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcriptions", x => x.SubcriptionId);
                });

            migrationBuilder.CreateTable(
                name: "UserSubcriptions",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubcriptionId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubcriptions", x => new { x.UserId, x.SubcriptionId });
                    table.ForeignKey(
                        name: "FK_UserSubcriptions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSubcriptions_Subcriptions_SubcriptionId",
                        column: x => x.SubcriptionId,
                        principalTable: "Subcriptions",
                        principalColumn: "SubcriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("307aa882-1854-4519-9ddc-fccb6edc0ff3"), null, "Moderator", "MODERATOR" },
                    { new Guid("4174a15f-5f82-44dc-b4ec-5e106e1df91f"), null, "Admin", "ADMIN" },
                    { new Guid("7b22f5a0-fbe9-4cfe-b745-e2ded7722b98"), null, "BasicMember", "BASICMEMBER" },
                    { new Guid("7e45ebed-2434-4f48-a667-e2d1f7e72079"), null, "PlusMember", "PLUSMEMBER" },
                    { new Guid("9299c795-3a2d-43f7-ba6e-26491cd5b5eb"), null, "ProMember", "PROMEMBER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSubcriptions_SubcriptionId",
                table: "UserSubcriptions",
                column: "SubcriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSubcriptions");

            migrationBuilder.DropTable(
                name: "Subcriptions");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("307aa882-1854-4519-9ddc-fccb6edc0ff3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4174a15f-5f82-44dc-b4ec-5e106e1df91f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7b22f5a0-fbe9-4cfe-b745-e2ded7722b98"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e45ebed-2434-4f48-a667-e2d1f7e72079"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9299c795-3a2d-43f7-ba6e-26491cd5b5eb"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("33a2f609-396a-4fa0-aaf6-8b2256767496"), null, "ProMember", "PROMEMBER" },
                    { new Guid("9448c7d6-93ff-4bbf-9352-636bb411889a"), null, "Admin", "ADMIN" },
                    { new Guid("a412f71d-40c7-4bbc-b267-1b01e4fe5f4c"), null, "Moderator", "MODERATOR" },
                    { new Guid("a462a7d5-f20b-4e23-92b8-f65866a8a2cc"), null, "BasicMember", "BASICMEMBER" },
                    { new Guid("b0887849-c0f1-472e-8cb8-c8c9ee758e5a"), null, "PlusMember", "PLUSMEMBER" }
                });
        }
    }
}
