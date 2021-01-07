using Microsoft.EntityFrameworkCore.Migrations;

namespace FBMS.Infrastructure.Migrations
{
    public partial class Add_Name_To_TransactionTemplateItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TransactionTemplateItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "TransactionTemplateItems");
        }
    }
}
