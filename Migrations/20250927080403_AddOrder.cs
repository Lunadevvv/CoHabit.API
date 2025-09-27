using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class AddOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5eed1339-6e60-454d-b5f9-dfb27493b0ea"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a4600456-a056-4801-a85e-2ea2a847bbb0"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b1440fc6-0a88-49eb-861b-8dd725a4c00d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d9c91a11-d0b1-44db-a497-8f6ffd2631b4"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f6c96e44-a5c6-4188-b21c-b80a990db220"));

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("10d61835-cabd-476c-be58-918358870151"), null, "Admin", "ADMIN" },
                    { new Guid("6e6b885c-8637-4b7d-854e-f04e92da9b95"), null, "ProMember", "PROMEMBER" },
                    { new Guid("88db4d41-afb7-448a-ae76-b782a7515bf3"), null, "PlusMember", "PLUSMEMBER" },
                    { new Guid("9d02ca9a-db39-4d50-a57a-7b81a6f837f4"), null, "BasicMember", "BASICMEMBER" },
                    { new Guid("de59cb26-efb5-4960-9704-25a385e87c6c"), null, "Moderator", "MODERATOR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PostId",
                table: "Orders",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("10d61835-cabd-476c-be58-918358870151"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6e6b885c-8637-4b7d-854e-f04e92da9b95"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("88db4d41-afb7-448a-ae76-b782a7515bf3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9d02ca9a-db39-4d50-a57a-7b81a6f837f4"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("de59cb26-efb5-4960-9704-25a385e87c6c"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5eed1339-6e60-454d-b5f9-dfb27493b0ea"), null, "BasicMember", "BASICMEMBER" },
                    { new Guid("a4600456-a056-4801-a85e-2ea2a847bbb0"), null, "PlusMember", "PLUSMEMBER" },
                    { new Guid("b1440fc6-0a88-49eb-861b-8dd725a4c00d"), null, "ProMember", "PROMEMBER" },
                    { new Guid("d9c91a11-d0b1-44db-a497-8f6ffd2631b4"), null, "Moderator", "MODERATOR" },
                    { new Guid("f6c96e44-a5c6-4188-b21c-b80a990db220"), null, "Admin", "ADMIN" }
                });
        }
    }
}
