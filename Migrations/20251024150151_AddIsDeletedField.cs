using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("560c8db2-f08d-447d-a263-3b9c92c02dd5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("af72d639-bebe-4b8c-8e26-05e333b6af5c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d0403e66-74cc-4ecd-914e-be8cf21a4b07"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d75b3772-1702-4b2c-a06f-c81e004cfbda"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("efd20dd3-c63d-4495-9be2-330edd17b1ab"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserFeedbacks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PostFeedbacks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("1e9c0094-45f7-49e0-8e30-d8531dd0b3c3"), null, "ProMember", "PROMEMBER" },
                    { new Guid("34f62bb2-6fca-4640-bbdb-4f130a447fb7"), null, "Moderator", "MODERATOR" },
                    { new Guid("8f713770-a388-4068-95f3-a7f7f4abf7a4"), null, "Admin", "ADMIN" },
                    { new Guid("bc49a624-e8d9-418d-b406-8bbc9871ae35"), null, "BasicMember", "BASICMEMBER" },
                    { new Guid("e3ebcfcf-14f8-4819-8c82-3033eb1f9260"), null, "PlusMember", "PLUSMEMBER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1e9c0094-45f7-49e0-8e30-d8531dd0b3c3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("34f62bb2-6fca-4640-bbdb-4f130a447fb7"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8f713770-a388-4068-95f3-a7f7f4abf7a4"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bc49a624-e8d9-418d-b406-8bbc9871ae35"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e3ebcfcf-14f8-4819-8c82-3033eb1f9260"));

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserFeedbacks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PostFeedbacks");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("560c8db2-f08d-447d-a263-3b9c92c02dd5"), null, "PlusMember", "PLUSMEMBER" },
                    { new Guid("af72d639-bebe-4b8c-8e26-05e333b6af5c"), null, "Admin", "ADMIN" },
                    { new Guid("d0403e66-74cc-4ecd-914e-be8cf21a4b07"), null, "Moderator", "MODERATOR" },
                    { new Guid("d75b3772-1702-4b2c-a06f-c81e004cfbda"), null, "ProMember", "PROMEMBER" },
                    { new Guid("efd20dd3-c63d-4495-9be2-330edd17b1ab"), null, "BasicMember", "BASICMEMBER" }
                });
        }
    }
}
