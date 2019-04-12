using Microsoft.EntityFrameworkCore.Migrations;

namespace SunridgeHOA.Migrations
{
    public partial class FileChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_ClassifiedListing_ClassifiedListingId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "FileStream",
                table: "File");

            migrationBuilder.DropColumn(
                name: "ImageContentType",
                table: "File");

            migrationBuilder.DropColumn(
                name: "IsMainImage",
                table: "File");

            migrationBuilder.AlterColumn<int>(
                name: "ClassifiedListingId",
                table: "File",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_File_ClassifiedListing_ClassifiedListingId",
                table: "File",
                column: "ClassifiedListingId",
                principalTable: "ClassifiedListing",
                principalColumn: "ClassifiedListingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_ClassifiedListing_ClassifiedListingId",
                table: "File");

            migrationBuilder.AlterColumn<int>(
                name: "ClassifiedListingId",
                table: "File",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileStream",
                table: "File",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageContentType",
                table: "File",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IsMainImage",
                table: "File",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_File_ClassifiedListing_ClassifiedListingId",
                table: "File",
                column: "ClassifiedListingId",
                principalTable: "ClassifiedListing",
                principalColumn: "ClassifiedListingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
