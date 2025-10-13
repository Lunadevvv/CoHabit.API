using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserPhoneNumberField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ__User__536C85E43394AC26",
                table: "AspNetUsers");

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

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "UQ__User__536C85E43394AC26",
                table: "AspNetUsers",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ__User__536C85E43394AC26",
                table: "AspNetUsers");

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

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.CreateIndex(
                name: "UQ__User__536C85E43394AC26",
                table: "AspNetUsers",
                column: "Phone",
                unique: true);
        }
    }
}
