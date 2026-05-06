using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class uhuhfdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReasonId",
                table: "IgnoredTrips",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BankList",
                columns: table => new
                {
                    BankListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankList", x => x.BankListId);
                });

            migrationBuilder.CreateTable(
                name: "Outstationinsurence",
                columns: table => new
                {
                    OutstationinsurenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    onewayPrice = table.Column<int>(type: "int", nullable: false),
                    Onewaypercentage = table.Column<int>(type: "int", nullable: false),
                    RoundtripPrice = table.Column<int>(type: "int", nullable: false),
                    Roundtrippercentage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outstationinsurence", x => x.OutstationinsurenceId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankList");

            migrationBuilder.DropTable(
                name: "Outstationinsurence");

            migrationBuilder.DropColumn(
                name: "ReasonId",
                table: "IgnoredTrips");
        }
    }
}
