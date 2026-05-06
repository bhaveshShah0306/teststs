using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoChauffeurWebApi.Migrations
{
    /// <inheritdoc />
    public partial class gudsd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Referals",
                newName: "ReferredTo");

            migrationBuilder.AddColumn<string>(
                name: "ReferCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReferedBy",
                table: "Referals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReferEarnings",
                columns: table => new
                {
                    ReferEarningId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferredEarning = table.Column<int>(type: "int", nullable: true),
                    ReferredByEarning = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferEarnings", x => x.ReferEarningId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferEarnings");

            migrationBuilder.DropColumn(
                name: "ReferCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReferedBy",
                table: "Referals");

            migrationBuilder.RenameColumn(
                name: "ReferredTo",
                table: "Referals",
                newName: "UserId");
        }
    }
}
