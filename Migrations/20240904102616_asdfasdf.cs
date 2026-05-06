using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class asdfasdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "WithdrawRequest",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlock",
                table: "Drivers",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Withdrawamountvalue",
                columns: table => new
                {
                    WithdrawamountvalueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinimumAmount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Withdrawamountvalue", x => x.WithdrawamountvalueId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Withdrawamountvalue");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "WithdrawRequest");

            migrationBuilder.DropColumn(
                name: "IsBlock",
                table: "Drivers");
        }
    }
}
