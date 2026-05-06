using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class jccdc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "selecteddateListvalue",
                table: "MonthlyDateLists",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "selecteddateListvalue",
                table: "FlexiDatesLists",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "selecteddateListvalue",
                table: "MonthlyDateLists");

            migrationBuilder.DropColumn(
                name: "selecteddateListvalue",
                table: "FlexiDatesLists");
        }
    }
}
