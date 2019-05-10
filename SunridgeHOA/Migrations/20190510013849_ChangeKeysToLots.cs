using Microsoft.EntityFrameworkCore.Migrations;

namespace SunridgeHOA.Migrations
{
    public partial class ChangeKeysToLots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeyHistory_Owner_OwnerId",
                table: "KeyHistory");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "KeyHistory",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "LotId",
                table: "KeyHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_KeyHistory_LotId",
                table: "KeyHistory",
                column: "LotId");

            migrationBuilder.AddForeignKey(
                name: "FK_KeyHistory_Lot_LotId",
                table: "KeyHistory",
                column: "LotId",
                principalTable: "Lot",
                principalColumn: "LotId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KeyHistory_Owner_OwnerId",
                table: "KeyHistory",
                column: "OwnerId",
                principalTable: "Owner",
                principalColumn: "OwnerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeyHistory_Lot_LotId",
                table: "KeyHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_KeyHistory_Owner_OwnerId",
                table: "KeyHistory");

            migrationBuilder.DropIndex(
                name: "IX_KeyHistory_LotId",
                table: "KeyHistory");

            migrationBuilder.DropColumn(
                name: "LotId",
                table: "KeyHistory");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "KeyHistory",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KeyHistory_Owner_OwnerId",
                table: "KeyHistory",
                column: "OwnerId",
                principalTable: "Owner",
                principalColumn: "OwnerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
