using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SunridgeHOA.Migrations
{
    public partial class DbCleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_OwnerHistory_OwnerHistoryId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_File_ClassifiedListing_ClassifiedListingId",
                table: "File");

            migrationBuilder.DropForeignKey(
                name: "FK_File_LotHistory_LotHistoryId",
                table: "File");

            migrationBuilder.DropForeignKey(
                name: "FK_File_OwnerHistory_OwnerHistoryId",
                table: "File");

            migrationBuilder.DropForeignKey(
                name: "FK_LotHistory_HistoryType_HistoryTypeId",
                table: "LotHistory");

            migrationBuilder.DropTable(
                name: "OwnerContactType");

            migrationBuilder.DropTable(
                name: "OwnerHistory");

            migrationBuilder.DropTable(
                name: "ContactType");

            migrationBuilder.DropTable(
                name: "HistoryType");

            migrationBuilder.DropIndex(
                name: "IX_LotHistory_HistoryTypeId",
                table: "LotHistory");

            migrationBuilder.DropIndex(
                name: "IX_File_ClassifiedListingId",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_File_OwnerHistoryId",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_Comment_OwnerHistoryId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "LotHistory");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LotHistory");

            migrationBuilder.DropColumn(
                name: "HistoryTypeId",
                table: "LotHistory");

            migrationBuilder.DropColumn(
                name: "ClassifiedListingId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "File");

            migrationBuilder.DropColumn(
                name: "OwnerHistoryId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "OwnerHistoryId",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "File",
                newName: "Name");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_LotHistory_LotHistoryId",
                table: "File");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "File",
                newName: "Type");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "LotHistory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LotHistory",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HistoryTypeId",
                table: "LotHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "LotHistoryId",
                table: "File",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ClassifiedListingId",
                table: "File",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "File",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OwnerHistoryId",
                table: "File",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerHistoryId",
                table: "Comment",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContactType",
                columns: table => new
                {
                    ContactTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsArchive = table.Column<bool>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactType", x => x.ContactTypeId);
                });

            migrationBuilder.CreateTable(
                name: "HistoryType",
                columns: table => new
                {
                    HistoryTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    IsArchive = table.Column<bool>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryType", x => x.HistoryTypeId);
                });

            migrationBuilder.CreateTable(
                name: "OwnerContactType",
                columns: table => new
                {
                    OwnerContactTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContactTypeId = table.Column<int>(nullable: false),
                    ContactValue = table.Column<string>(nullable: true),
                    IsArchive = table.Column<bool>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerContactType", x => x.OwnerContactTypeId);
                    table.ForeignKey(
                        name: "FK_OwnerContactType_ContactType_ContactTypeId",
                        column: x => x.ContactTypeId,
                        principalTable: "ContactType",
                        principalColumn: "ContactTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OwnerContactType_Owner_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owner",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OwnerHistory",
                columns: table => new
                {
                    OwnerHistoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    HistoryTypeId = table.Column<int>(nullable: false),
                    IsArchive = table.Column<bool>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<int>(nullable: false),
                    PrivacyLevel = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerHistory", x => x.OwnerHistoryId);
                    table.ForeignKey(
                        name: "FK_OwnerHistory_HistoryType_HistoryTypeId",
                        column: x => x.HistoryTypeId,
                        principalTable: "HistoryType",
                        principalColumn: "HistoryTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OwnerHistory_Owner_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owner",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LotHistory_HistoryTypeId",
                table: "LotHistory",
                column: "HistoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_File_ClassifiedListingId",
                table: "File",
                column: "ClassifiedListingId");

            migrationBuilder.CreateIndex(
                name: "IX_File_OwnerHistoryId",
                table: "File",
                column: "OwnerHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_OwnerHistoryId",
                table: "Comment",
                column: "OwnerHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerContactType_ContactTypeId",
                table: "OwnerContactType",
                column: "ContactTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerContactType_OwnerId",
                table: "OwnerContactType",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerHistory_HistoryTypeId",
                table: "OwnerHistory",
                column: "HistoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerHistory_OwnerId",
                table: "OwnerHistory",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_OwnerHistory_OwnerHistoryId",
                table: "Comment",
                column: "OwnerHistoryId",
                principalTable: "OwnerHistory",
                principalColumn: "OwnerHistoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_File_ClassifiedListing_ClassifiedListingId",
                table: "File",
                column: "ClassifiedListingId",
                principalTable: "ClassifiedListing",
                principalColumn: "ClassifiedListingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_File_LotHistory_LotHistoryId",
                table: "File",
                column: "LotHistoryId",
                principalTable: "LotHistory",
                principalColumn: "LotHistoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_File_OwnerHistory_OwnerHistoryId",
                table: "File",
                column: "OwnerHistoryId",
                principalTable: "OwnerHistory",
                principalColumn: "OwnerHistoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LotHistory_HistoryType_HistoryTypeId",
                table: "LotHistory",
                column: "HistoryTypeId",
                principalTable: "HistoryType",
                principalColumn: "HistoryTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
