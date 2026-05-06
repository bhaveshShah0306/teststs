using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class jhuhdfd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAtDrop",
                table: "Trips",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAtDrop",
                table: "MonthlyDateLists",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAtDrop",
                table: "FlexiDatesLists",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAtDrop",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "IsAtDrop",
                table: "MonthlyDateLists");

            migrationBuilder.DropColumn(
                name: "IsAtDrop",
                table: "FlexiDatesLists");
        }
    }
}
