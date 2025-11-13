using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAverageRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Posts",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("1af1ca73-5732-4e27-8da6-df410b587799"), null, "ProMember", "PROMEMBER" },
                    { new Guid("6c23519d-7d27-4e2b-b2ad-444d1a505134"), null, "Admin", "ADMIN" },
                    { new Guid("c8a9fddf-6690-470e-b056-57590d89df1e"), null, "Moderator", "MODERATOR" },
                    { new Guid("ccd3e88a-d586-4fdd-ae94-c057fe52ceac"), null, "BasicMember", "BASICMEMBER" },
                    { new Guid("e0804115-3b67-4c99-8df4-feeeb40110a9"), null, "PlusMember", "PLUSMEMBER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1af1ca73-5732-4e27-8da6-df410b587799"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6c23519d-7d27-4e2b-b2ad-444d1a505134"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c8a9fddf-6690-470e-b056-57590d89df1e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ccd3e88a-d586-4fdd-ae94-c057fe52ceac"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e0804115-3b67-4c99-8df4-feeeb40110a9"));

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Posts");

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
    }
}
