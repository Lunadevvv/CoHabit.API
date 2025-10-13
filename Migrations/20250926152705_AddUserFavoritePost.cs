using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserFavoritePost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("07e7d895-1a84-492d-8210-1b68a6440c65"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0d387877-be2a-4ddc-963b-cc4c55504988"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5926dc4d-aed7-4cc6-b9b5-725572ad01b1"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6b7c6c65-108b-4ca0-b4fd-0ac664356637"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a00d9c3a-1e42-4cfd-9ef6-8c57c37d80d3"));

            migrationBuilder.CreateTable(
                name: "UserFavoritePosts",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoritePosts", x => new { x.UserId, x.PostId });
                    table.ForeignKey(
                        name: "FK_UserFavoritePost_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId");
                    table.ForeignKey(
                        name: "FK_UserFavoritePost_User_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5eed1339-6e60-454d-b5f9-dfb27493b0ea"), null, "BasicMember", "BASICMEMBER" },
                    { new Guid("a4600456-a056-4801-a85e-2ea2a847bbb0"), null, "PlusMember", "PLUSMEMBER" },
                    { new Guid("b1440fc6-0a88-49eb-861b-8dd725a4c00d"), null, "ProMember", "PROMEMBER" },
                    { new Guid("d9c91a11-d0b1-44db-a497-8f6ffd2631b4"), null, "Moderator", "MODERATOR" },
                    { new Guid("f6c96e44-a5c6-4188-b21c-b80a990db220"), null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoritePosts_PostId",
                table: "UserFavoritePosts",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavoritePosts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5eed1339-6e60-454d-b5f9-dfb27493b0ea"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a4600456-a056-4801-a85e-2ea2a847bbb0"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b1440fc6-0a88-49eb-861b-8dd725a4c00d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d9c91a11-d0b1-44db-a497-8f6ffd2631b4"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f6c96e44-a5c6-4188-b21c-b80a990db220"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("07e7d895-1a84-492d-8210-1b68a6440c65"), null, "PlusMember", "PLUSMEMBER" },
                    { new Guid("0d387877-be2a-4ddc-963b-cc4c55504988"), null, "Moderator", "MODERATOR" },
                    { new Guid("5926dc4d-aed7-4cc6-b9b5-725572ad01b1"), null, "ProMember", "PROMEMBER" },
                    { new Guid("6b7c6c65-108b-4ca0-b4fd-0ac664356637"), null, "Admin", "ADMIN" },
                    { new Guid("a00d9c3a-1e42-4cfd-9ef6-8c57c37d80d3"), null, "BasicMember", "BASICMEMBER" }
                });
        }
    }
}
