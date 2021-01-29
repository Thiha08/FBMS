using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FBMS.Infrastructure.Migrations
{
    public partial class TransactionDischarged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DischargedDate",
                table: "Transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDischarged",
                table: "Transactions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DischargedDate",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "IsDischarged",
                table: "Transactions");
        }
    }
}
