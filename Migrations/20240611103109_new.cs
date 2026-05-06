using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CloseTrip",
                table: "Monthlies",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CloseTrip",
                table: "Flexis",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseTrip",
                table: "Monthlies");

            migrationBuilder.DropColumn(
                name: "CloseTrip",
                table: "Flexis");
        }
    }
}
