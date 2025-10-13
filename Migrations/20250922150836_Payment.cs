using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class Payment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ__Otp__C8EE201F536C85E4",
                table: "Otps");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("02de5e89-a336-4c7a-93d1-4de56e24c233"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0c956220-0e7e-40cd-ab22-0f68fa77e6f7"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("43ddb33f-a6f6-4153-b7ea-9755467b44ef"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c832d41e-f65c-46a5-900f-165791a14296"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f5f774b4-bdd2-4c79-8d9d-3159ed023a0c"));

            migrationBuilder.AlterColumn<string>(
                name: "CodeHashed",
                table: "Otps",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("036d6ce1-fd37-441e-b81a-4b616243bae7"), null, "ProMember", "PROMEMBER" },
                    { new Guid("1fe40792-bb6e-4244-b29e-933c9549f1f6"), null, "BasicMember", "BASICMEMBER" },
                    { new Guid("baee4f0a-706e-41b5-be6b-1094a74d3ba1"), null, "Admin", "ADMIN" },
                    { new Guid("c12f2f07-f7a1-48c0-8abc-b4729c1dfdfc"), null, "Moderator", "MODERATOR" },
                    { new Guid("d6fee7d6-29a1-4c6d-ab65-2d2b964da6c4"), null, "PlusMember", "PLUSMEMBER" }
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Otp__C8EE201F536C85E4",
                table: "Otps",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropIndex(
                name: "UQ__Otp__C8EE201F536C85E4",
                table: "Otps");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("036d6ce1-fd37-441e-b81a-4b616243bae7"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1fe40792-bb6e-4244-b29e-933c9549f1f6"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("baee4f0a-706e-41b5-be6b-1094a74d3ba1"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c12f2f07-f7a1-48c0-8abc-b4729c1dfdfc"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d6fee7d6-29a1-4c6d-ab65-2d2b964da6c4"));

            migrationBuilder.AlterColumn<string>(
                name: "CodeHashed",
                table: "Otps",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("02de5e89-a336-4c7a-93d1-4de56e24c233"), null, "Moderator", "MODERATOR" },
                    { new Guid("0c956220-0e7e-40cd-ab22-0f68fa77e6f7"), null, "PlusMember", "PLUSMEMBER" },
                    { new Guid("43ddb33f-a6f6-4153-b7ea-9755467b44ef"), null, "Admin", "ADMIN" },
                    { new Guid("c832d41e-f65c-46a5-900f-165791a14296"), null, "BasicMember", "BASICMEMBER" },
                    { new Guid("f5f774b4-bdd2-4c79-8d9d-3159ed023a0c"), null, "ProMember", "PROMEMBER" }
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Otp__C8EE201F536C85E4",
                table: "Otps",
                columns: new[] { "Phone", "CodeHashed" },
                unique: true);
        }
    }
}
