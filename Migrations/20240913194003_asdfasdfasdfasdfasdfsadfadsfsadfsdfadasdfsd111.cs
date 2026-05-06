using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class asdfasdfasdfasdfasdfsadfadsfsadfsdfadasdfsd111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaymentdone",
                table: "Trips",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaymentdone",
                table: "MonthlyDateLists",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaymentdone",
                table: "FlexiDatesLists",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaymentdone",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "IsPaymentdone",
                table: "MonthlyDateLists");

            migrationBuilder.DropColumn(
                name: "IsPaymentdone",
                table: "FlexiDatesLists");
        }
    }
}
