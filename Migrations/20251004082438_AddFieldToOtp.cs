using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldToOtp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4c40c612-1fd7-4211-95be-b033dc05d1eb"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5e350020-ba87-4de4-9ff0-a5b06b3e502c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a7176802-192e-41e9-8010-511050974553"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b26a1026-903b-4b73-bb1f-c4838768ac0b"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bf339fef-069b-4cf7-b196-9eeec93ca35d"));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Otps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5596b386-7016-4d8b-9b54-3e58137f7155"), null, "PlusMember", "PLUSMEMBER" },
                    { new Guid("9dd8594c-208b-4e65-88ad-204b8f6998c3"), null, "BasicMember", "BASICMEMBER" },
                    { new Guid("c0615e8f-06c1-407c-a867-f371b50bd807"), null, "Admin", "ADMIN" },
                    { new Guid("c8766529-32b8-44eb-8a2e-ae75df691be3"), null, "Moderator", "MODERATOR" },
                    { new Guid("e3762c2e-6a07-47f9-8de9-b60bad866a32"), null, "ProMember", "PROMEMBER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5596b386-7016-4d8b-9b54-3e58137f7155"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9dd8594c-208b-4e65-88ad-204b8f6998c3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c0615e8f-06c1-407c-a867-f371b50bd807"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c8766529-32b8-44eb-8a2e-ae75df691be3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e3762c2e-6a07-47f9-8de9-b60bad866a32"));

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Otps");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("4c40c612-1fd7-4211-95be-b033dc05d1eb"), null, "PlusMember", "PLUSMEMBER" },
                    { new Guid("5e350020-ba87-4de4-9ff0-a5b06b3e502c"), null, "Admin", "ADMIN" },
                    { new Guid("a7176802-192e-41e9-8010-511050974553"), null, "ProMember", "PROMEMBER" },
                    { new Guid("b26a1026-903b-4b73-bb1f-c4838768ac0b"), null, "Moderator", "MODERATOR" },
                    { new Guid("bf339fef-069b-4cf7-b196-9eeec93ca35d"), null, "BasicMember", "BASICMEMBER" }
                });
        }
    }
}
