using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KLIX_Link.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_atribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilesId",
                table: "_Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<int>>(
                name: "FilesId",
                table: "_Users",
                type: "integer[]",
                nullable: false);
        }
    }
}
