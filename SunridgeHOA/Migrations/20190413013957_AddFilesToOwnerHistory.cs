using Microsoft.EntityFrameworkCore.Migrations;

namespace SunridgeHOA.Migrations
{
    public partial class AddFilesToOwnerHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerHistoryId",
                table: "File",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_File_OwnerHistoryId",
                table: "File",
                column: "OwnerHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_OwnerHistory_OwnerHistoryId",
                table: "File",
                column: "OwnerHistoryId",
                principalTable: "OwnerHistory",
                principalColumn: "OwnerHistoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_OwnerHistory_OwnerHistoryId",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_File_OwnerHistoryId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "OwnerHistoryId",
                table: "File");
        }
    }
}
