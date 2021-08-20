using Microsoft.EntityFrameworkCore.Migrations;

namespace FBMS.Infrastructure.Migrations
{
    public partial class InsertNewSettingsForClientSide : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[Settings] ([Name], [Key], [Value], [DateCreated], [DateUpdated], [Status]) VALUES ('ClientApiCrawlerSettings', 'AcceptablePassedMinute', '5', '2021-08-20 00:00:00.0000000', '2021-08-20 00:00:00.0000000', 1);");
            migrationBuilder.Sql("INSERT INTO [dbo].[Settings] ([Name], [Key], [Value], [DateCreated], [DateUpdated], [Status]) VALUES ('ClientApiCrawlerSettings', 'AcceptableDischargedCount', '3', '2021-08-20 00:00:00.0000000', '2021-08-20 00:00:00.0000000', 1);");
            migrationBuilder.Sql("INSERT INTO [dbo].[Settings] ([Name], [Key], [Value], [DateCreated], [DateUpdated], [Status]) VALUES ('ClientApiCrawlerSettings', 'IsTestingStack', 'True', '2021-08-20 00:00:00.0000000', '2021-08-20 00:00:00.0000000', 1);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE [dbo].[Settings] WHERE [Name] = 'ClientApiCrawlerSettings' AND [Key] = 'AcceptablePassedMinute';");
            migrationBuilder.Sql("DELETE [dbo].[Settings] WHERE [Name] = 'ClientApiCrawlerSettings' AND [Key] = 'AcceptableDischargedCount';");
            migrationBuilder.Sql("DELETE [dbo].[Settings] WHERE [Name] = 'ClientApiCrawlerSettings' AND [Key] = 'IsTestingStack';");
        }
    }
}
