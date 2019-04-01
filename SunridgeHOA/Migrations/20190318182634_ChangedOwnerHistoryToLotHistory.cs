using Microsoft.EntityFrameworkCore.Migrations;

namespace SunridgeHOA.Migrations
{
    public partial class ChangedOwnerHistoryToLotHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_OwnerHistory_OwnerHistoryId",
                table: "File");

            migrationBuilder.RenameColumn(
                name: "OwnerHistoryId",
                table: "OwnerHistory",
                newName: "LotHistoryId");

            migrationBuilder.RenameColumn(
                name: "OwnerHistoryId",
                table: "File",
                newName: "LotHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_File_OwnerHistoryId",
                table: "File",
                newName: "IX_File_LotHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_OwnerHistory_LotHistoryId",
                table: "File",
                column: "LotHistoryId",
                principalTable: "OwnerHistory",
                principalColumn: "LotHistoryId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_OwnerHistory_LotHistoryId",
                table: "File");

            migrationBuilder.RenameColumn(
                name: "LotHistoryId",
                table: "OwnerHistory",
                newName: "OwnerHistoryId");

            migrationBuilder.RenameColumn(
                name: "LotHistoryId",
                table: "File",
                newName: "OwnerHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_File_LotHistoryId",
                table: "File",
                newName: "IX_File_OwnerHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_OwnerHistory_OwnerHistoryId",
                table: "File",
                column: "OwnerHistoryId",
                principalTable: "OwnerHistory",
                principalColumn: "OwnerHistoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
