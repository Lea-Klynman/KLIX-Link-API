using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KLIX_Link.Data.Migrations
{
    /// <inheritdoc />
    public partial class DigSignature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "_Files",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Signature",
                table: "_Files");
        }
    }
}
