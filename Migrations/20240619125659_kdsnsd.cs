using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class kdsnsd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsersTripsCancelResonsId",
                table: "Monthlies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersTripsCancelResonsId",
                table: "Flexis",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsersTripsCancelResonsId",
                table: "Monthlies");

            migrationBuilder.DropColumn(
                name: "UsersTripsCancelResonsId",
                table: "Flexis");
        }
    }
}
