using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KLIX_Link.Data.Migrations
{
    /// <inheritdoc />
    public partial class registerPostModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "EmailAloowed",
                table: "_Files",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);

            migrationBuilder.CreateIndex(
                name: "IX__Users_Email",
                table: "_Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX__Users_Email",
                table: "_Users");

            migrationBuilder.DropColumn(
                name: "EmailAloowed",
                table: "_Files");
        }
    }
}
