using Microsoft.EntityFrameworkCore.Migrations;

namespace SunridgeHOA.Migrations
{
    public partial class RemoveFieldFromFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormHistoryId",
                table: "File");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormHistoryId",
                table: "File",
                nullable: true);
        }
    }
}
