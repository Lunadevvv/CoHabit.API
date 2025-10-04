using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTypeOfPaymentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Payments",
                type: "int",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentId",
                table: "Payments",
                type: "nvarchar(26)",
                maxLength: 26,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentId",
                table: "Payments",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(26)",
                oldMaxLength: 26);

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
    }
}
