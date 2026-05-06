using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class bubss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "Monthlies",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessing",
                table: "Monthlies",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReserved",
                table: "Monthlies",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTripCompByDriver",
                table: "Monthlies",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTripStarted",
                table: "Monthlies",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsdriverArrived",
                table: "Monthlies",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "Flexis",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessing",
                table: "Flexis",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReserved",
                table: "Flexis",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTripCompByDriver",
                table: "Flexis",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTripStarted",
                table: "Flexis",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsdriverArrived",
                table: "Flexis",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "Monthlies");

            migrationBuilder.DropColumn(
                name: "IsProcessing",
                table: "Monthlies");

            migrationBuilder.DropColumn(
                name: "IsReserved",
                table: "Monthlies");

            migrationBuilder.DropColumn(
                name: "IsTripCompByDriver",
                table: "Monthlies");

            migrationBuilder.DropColumn(
                name: "IsTripStarted",
                table: "Monthlies");

            migrationBuilder.DropColumn(
                name: "IsdriverArrived",
                table: "Monthlies");

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "Flexis");

            migrationBuilder.DropColumn(
                name: "IsProcessing",
                table: "Flexis");

            migrationBuilder.DropColumn(
                name: "IsReserved",
                table: "Flexis");

            migrationBuilder.DropColumn(
                name: "IsTripCompByDriver",
                table: "Flexis");

            migrationBuilder.DropColumn(
                name: "IsTripStarted",
                table: "Flexis");

            migrationBuilder.DropColumn(
                name: "IsdriverArrived",
                table: "Flexis");
        }
    }
}
