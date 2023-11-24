using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Data.Migrations
{
    /// <inheritdoc />
    public partial class addBlogUserImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "AspNetUsers",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageType",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "AspNetUsers");
        }
    }
}
