using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FBMS.Infrastructure.Migrations
{
    public partial class Submitted_Transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSubmitted",
                table: "Transactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "SubmittedAmount",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedDate",
                table: "Transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubmittedPricing",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubmittedTransactionType",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubmitted",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SubmittedAmount",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SubmittedDate",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SubmittedPricing",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SubmittedTransactionType",
                table: "Transactions");
        }
    }
}
