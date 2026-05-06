using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class nfihfefe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "DriverWalletTransactionHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "DriverTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "DriverWalletTransactionHistories");

            migrationBuilder.DropColumn(
                name: "description",
                table: "DriverTransactions");
        }
    }
}
