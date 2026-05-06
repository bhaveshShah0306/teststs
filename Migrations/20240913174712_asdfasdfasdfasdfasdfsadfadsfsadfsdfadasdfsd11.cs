using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class asdfasdfasdfasdfasdfsadfadsfsadfsdfadasdfsd11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEndPicsTaken",
                table: "MonthlyDateLists",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Istakenpics",
                table: "MonthlyDateLists",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEndPicsTaken",
                table: "FlexiDatesLists",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Istakenpics",
                table: "FlexiDatesLists",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEndPicsTaken",
                table: "MonthlyDateLists");

            migrationBuilder.DropColumn(
                name: "Istakenpics",
                table: "MonthlyDateLists");

            migrationBuilder.DropColumn(
                name: "IsEndPicsTaken",
                table: "FlexiDatesLists");

            migrationBuilder.DropColumn(
                name: "Istakenpics",
                table: "FlexiDatesLists");
        }
    }
}
