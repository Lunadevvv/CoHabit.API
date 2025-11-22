using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoHabit.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAppFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppFeedbacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FeedbackText = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    ExperienceScore = table.Column<int>(type: "integer", nullable: false),
                    MostFavoriteFeature = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppFeedbacks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppFeedbacks_UserId",
                table: "AppFeedbacks",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppFeedbacks");
        }
    }
}
