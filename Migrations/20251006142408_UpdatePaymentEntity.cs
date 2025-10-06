using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "PaymentLinkId",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("33a2f609-396a-4fa0-aaf6-8b2256767496"), null, "ProMember", "PROMEMBER" },
                    { new Guid("9448c7d6-93ff-4bbf-9352-636bb411889a"), null, "Admin", "ADMIN" },
                    { new Guid("a412f71d-40c7-4bbc-b267-1b01e4fe5f4c"), null, "Moderator", "MODERATOR" },
                    { new Guid("a462a7d5-f20b-4e23-92b8-f65866a8a2cc"), null, "BasicMember", "BASICMEMBER" },
                    { new Guid("b0887849-c0f1-472e-8cb8-c8c9ee758e5a"), null, "PlusMember", "PLUSMEMBER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentLinkId",
                table: "Payments",
                column: "PaymentLinkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payment_PaymentLinkId",
                table: "Payments");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("33a2f609-396a-4fa0-aaf6-8b2256767496"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9448c7d6-93ff-4bbf-9352-636bb411889a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a412f71d-40c7-4bbc-b267-1b01e4fe5f4c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a462a7d5-f20b-4e23-92b8-f65866a8a2cc"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b0887849-c0f1-472e-8cb8-c8c9ee758e5a"));

            migrationBuilder.DropColumn(
                name: "PaymentLinkId",
                table: "Payments");

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
    }
}
