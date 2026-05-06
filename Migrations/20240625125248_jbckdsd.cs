using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class jbckdsd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AfterGsttotalsubamount",
                table: "Subscriptions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DriverPenalties",
                columns: table => new
                {
                    DriverPenaltyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Balance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverPenalties", x => x.DriverPenaltyId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverPenalties");

            migrationBuilder.DropColumn(
                name: "AfterGsttotalsubamount",
                table: "Subscriptions");
        }
    }
}
