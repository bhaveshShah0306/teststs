using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class asdfasdfasdfasdfasdfsadfadsfsadfsdfadasdfsd1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEndPicsTaken",
                table: "Trips",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Istakenpics",
                table: "Trips",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEndPicsTaken",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Istakenpics",
                table: "Trips");
        }
    }
}
