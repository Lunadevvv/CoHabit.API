using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserRefreshTokenField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("50a3265c-41e8-4c31-80c9-7b245eef1fe1"), null, "ProMember", "PROMEMBER" },
                    { new Guid("6435bbbf-e3c6-403b-a874-57f4e483fb5b"), null, "Admin", "ADMIN" },
                    { new Guid("900c6835-d4aa-42fc-b34f-70ee724aadaf"), null, "BasicMember", "BASICMEMBER" },
                    { new Guid("93c8d7b2-e056-4ee6-81e3-8f0b9a8141f6"), null, "Moderator", "MODERATOR" },
                    { new Guid("a9a4bc1a-65c9-4c2c-911f-c5dca6d30999"), null, "PlusMember", "PLUSMEMBER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("50a3265c-41e8-4c31-80c9-7b245eef1fe1"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6435bbbf-e3c6-403b-a874-57f4e483fb5b"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("900c6835-d4aa-42fc-b34f-70ee724aadaf"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("93c8d7b2-e056-4ee6-81e3-8f0b9a8141f6"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a9a4bc1a-65c9-4c2c-911f-c5dca6d30999"));

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
        }
    }
}
