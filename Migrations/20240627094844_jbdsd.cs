using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class jbdsd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Isonroute",
                table: "Monthlies",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Isonroute",
                table: "Flexis",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Isonroute",
                table: "Monthlies");

            migrationBuilder.DropColumn(
                name: "Isonroute",
                table: "Flexis");
        }
    }
}
