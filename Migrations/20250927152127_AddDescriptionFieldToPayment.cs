using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionFieldToPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("3471ac75-bb09-45e2-a7ed-5a8c5fc79970"), null, "BasicMember", "BASICMEMBER" },
                    { new Guid("61aa04ca-24f4-4e9d-8e61-88cc9ada56b6"), null, "PlusMember", "PLUSMEMBER" },
                    { new Guid("63ac2d22-198c-4cf8-b34a-346131af7976"), null, "Moderator", "MODERATOR" },
                    { new Guid("ce7fe6ce-ae2d-490a-a071-e935aebc25b9"), null, "Admin", "ADMIN" },
                    { new Guid("e74b2d6c-4f86-45d5-ac2e-b3e76ad38782"), null, "ProMember", "PROMEMBER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3471ac75-bb09-45e2-a7ed-5a8c5fc79970"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("61aa04ca-24f4-4e9d-8e61-88cc9ada56b6"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("63ac2d22-198c-4cf8-b34a-346131af7976"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ce7fe6ce-ae2d-490a-a071-e935aebc25b9"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e74b2d6c-4f86-45d5-ac2e-b3e76ad38782"));

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Payments");

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
        }
    }
}
