using Microsoft.EntityFrameworkCore.Migrations;

namespace SunridgeHOA.Migrations
{
    public partial class AddApplicationUserToOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "Owner",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Owner");
        }
    }
}
