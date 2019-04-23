using Microsoft.EntityFrameworkCore.Migrations;

namespace SunridgeHOA.Migrations
{
    public partial class FixFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_LotHistory_LotHistoryId",
                table: "File");

            migrationBuilder.AlterColumn<int>(
                name: "LotHistoryId",
                table: "File",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_File_LotHistory_LotHistoryId",
                table: "File",
                column: "LotHistoryId",
                principalTable: "LotHistory",
                principalColumn: "LotHistoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_LotHistory_LotHistoryId",
                table: "File");

            migrationBuilder.AlterColumn<int>(
                name: "LotHistoryId",
                table: "File",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_File_LotHistory_LotHistoryId",
                table: "File",
                column: "LotHistoryId",
                principalTable: "LotHistory",
                principalColumn: "LotHistoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
