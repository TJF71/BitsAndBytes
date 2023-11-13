using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Data.Migrations
{
    /// <inheritdoc />
    public partial class databaseUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlogUserId",
                table: "BlogPlosts",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPlosts_BlogUserId",
                table: "BlogPlosts",
                column: "BlogUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPlosts_AspNetUsers_BlogUserId",
                table: "BlogPlosts",
                column: "BlogUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPlosts_AspNetUsers_BlogUserId",
                table: "BlogPlosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogPlosts_BlogUserId",
                table: "BlogPlosts");

            migrationBuilder.DropColumn(
                name: "BlogUserId",
                table: "BlogPlosts");
        }
    }
}
