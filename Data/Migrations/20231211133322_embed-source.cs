using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace efsane_oyun.Data.Migrations
{
    /// <inheritdoc />
    public partial class embedsource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmbedSource",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmbedSource",
                table: "Games");
        }
    }
}
