using Microsoft.EntityFrameworkCore.Migrations;

namespace FBMS.Infrastructure.Migrations
{
    public partial class DischargedCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DischargedCount",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DischargedCount",
                table: "Transactions");
        }
    }
}
