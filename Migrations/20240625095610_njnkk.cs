using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class njnkk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscripationGstId",
                table: "Driversubscriptions");

            migrationBuilder.AddColumn<int>(
                name: "SubscripationGstId",
                table: "Subscriptions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscripationGstId",
                table: "Subscriptions");

            migrationBuilder.AddColumn<int>(
                name: "SubscripationGstId",
                table: "Driversubscriptions",
                type: "int",
                nullable: true);
        }
    }
}
