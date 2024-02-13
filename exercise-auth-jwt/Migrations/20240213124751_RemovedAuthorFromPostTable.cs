using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace exercise_auth_jwt.Migrations
{
    /// <inheritdoc />
    public partial class RemovedAuthorFromPostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_blogpost_AspNetUsers_author_id",
                table: "blogpost");

            migrationBuilder.DropIndex(
                name: "IX_blogpost_author_id",
                table: "blogpost");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_blogpost_author_id",
                table: "blogpost",
                column: "author_id");

            migrationBuilder.AddForeignKey(
                name: "FK_blogpost_AspNetUsers_author_id",
                table: "blogpost",
                column: "author_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
