using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubcriptionFieldInPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<int>(
                name: "SubcriptionId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("020a1e3e-f047-4d7c-bb02-527b219b68d3"), null, "Moderator", "MODERATOR" },
                    { new Guid("131ec9db-25a5-4e19-b548-b9ec8c973b9d"), null, "BasicMember", "BASICMEMBER" },
                    { new Guid("587767cc-00cf-43e9-a566-0103cad518cf"), null, "Admin", "ADMIN" },
                    { new Guid("68e50412-b092-4f4b-8f51-5d06594ec0e9"), null, "PlusMember", "PLUSMEMBER" },
                    { new Guid("ad6ac0c6-9b1d-4802-8769-2e7812a62b69"), null, "ProMember", "PROMEMBER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("020a1e3e-f047-4d7c-bb02-527b219b68d3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("131ec9db-25a5-4e19-b548-b9ec8c973b9d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("587767cc-00cf-43e9-a566-0103cad518cf"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("68e50412-b092-4f4b-8f51-5d06594ec0e9"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ad6ac0c6-9b1d-4802-8769-2e7812a62b69"));

            migrationBuilder.DropColumn(
                name: "SubcriptionId",
                table: "Payments");

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
        }
    }
}
