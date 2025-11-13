using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2e11f535-051e-4d86-9ddc-3543c6eabfd4"), null, "Admin", "ADMIN" },
                    { new Guid("3416a9bb-49a6-420f-832f-78b197f57bc2"), null, "BasicMember", "BASICMEMBER" },
                    { new Guid("6a207f7c-9614-4fec-96e8-53cfe31ed8f2"), null, "Moderator", "MODERATOR" },
                    { new Guid("e15a9a60-10ae-4f93-acb7-3d38a4cc4125"), null, "PlusMember", "PLUSMEMBER" },
                    { new Guid("fc5ac104-d527-4d25-b9c9-358295d54ea4"), null, "ProMember", "PROMEMBER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2e11f535-051e-4d86-9ddc-3543c6eabfd4"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3416a9bb-49a6-420f-832f-78b197f57bc2"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a207f7c-9614-4fec-96e8-53cfe31ed8f2"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e15a9a60-10ae-4f93-acb7-3d38a4cc4125"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("fc5ac104-d527-4d25-b9c9-358295d54ea4"));
        }
    }
}
