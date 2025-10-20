using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPostImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "PostImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostImages_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2101968a-b5dc-4b13-9d87-02aa7a132d5e"), null, "BasicMember", "BASICMEMBER" },
                    { new Guid("241929fb-70d4-4268-b3ec-4f3b7da95b05"), null, "Moderator", "MODERATOR" },
                    { new Guid("46ce6dc1-49db-4c24-94bb-a114fbcbb55b"), null, "Admin", "ADMIN" },
                    { new Guid("4b36272a-d915-4529-bea8-af545ce9d38f"), null, "ProMember", "PROMEMBER" },
                    { new Guid("b5bcc62d-24f0-44eb-b11d-053e2f626c0c"), null, "PlusMember", "PLUSMEMBER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostImages_PostId",
                table: "PostImages",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostImages");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2101968a-b5dc-4b13-9d87-02aa7a132d5e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("241929fb-70d4-4268-b3ec-4f3b7da95b05"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("46ce6dc1-49db-4c24-94bb-a114fbcbb55b"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4b36272a-d915-4529-bea8-af545ce9d38f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b5bcc62d-24f0-44eb-b11d-053e2f626c0c"));

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
    }
}
