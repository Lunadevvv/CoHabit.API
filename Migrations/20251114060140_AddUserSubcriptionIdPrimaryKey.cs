using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSubcriptionIdPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSubcriptions",
                table: "UserSubcriptions");

            migrationBuilder.AddColumn<int>(
                name: "UserSubcriptionId",
                table: "UserSubcriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSubcriptions",
                table: "UserSubcriptions",
                column: "UserSubcriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubcriptions_UserId",
                table: "UserSubcriptions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSubcriptions",
                table: "UserSubcriptions");

            migrationBuilder.DropIndex(
                name: "IX_UserSubcriptions_UserId",
                table: "UserSubcriptions");

            migrationBuilder.DropColumn(
                name: "UserSubcriptionId",
                table: "UserSubcriptions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSubcriptions",
                table: "UserSubcriptions",
                columns: new[] { "UserId", "SubcriptionId" });
        }
    }
}
