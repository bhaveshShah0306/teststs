using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class jbsds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TripsUniqueId",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TripsUniqueId",
                table: "Trips");
        }
    }
}
