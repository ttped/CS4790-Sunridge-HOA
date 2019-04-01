using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SunridgeHOA.Migrations
{
    public partial class SeparatedHistoriesAddedOwnerLot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_OwnerHistory_LotHistoryId",
                table: "File");

            migrationBuilder.DropForeignKey(
                name: "FK_Lot_Owner_OwnerId",
                table: "Lot");

            migrationBuilder.DropForeignKey(
                name: "FK_Owner_Owner_CoOwnerId",
                table: "Owner");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnerHistory_Lot_LotId",
                table: "OwnerHistory");

            migrationBuilder.DropIndex(
                name: "IX_OwnerHistory_LotId",
                table: "OwnerHistory");

            migrationBuilder.DropIndex(
                name: "IX_Owner_CoOwnerId",
                table: "Owner");

            migrationBuilder.DropIndex(
                name: "IX_Lot_OwnerId",
                table: "Lot");

            migrationBuilder.DropColumn(
                name: "LogId",
                table: "OwnerHistory");

            migrationBuilder.DropColumn(
                name: "CoOwnerId",
                table: "Owner");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Lot");

            migrationBuilder.RenameColumn(
                name: "PrivacyLevel",
                table: "OwnerHistory",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "LotId",
                table: "OwnerHistory",
                newName: "Hours");

            migrationBuilder.RenameColumn(
                name: "LotHistoryId",
                table: "OwnerHistory",
                newName: "OwnerHistoryId");

            migrationBuilder.CreateTable(
                name: "LotHistory",
                columns: table => new
                {
                    LotHistoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LotId = table.Column<int>(nullable: false),
                    HistoryTypeId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    PrivacyLevel = table.Column<string>(nullable: true),
                    IsArchive = table.Column<bool>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotHistory", x => x.LotHistoryId);
                    table.ForeignKey(
                        name: "FK_LotHistory_HistoryType_HistoryTypeId",
                        column: x => x.HistoryTypeId,
                        principalTable: "HistoryType",
                        principalColumn: "HistoryTypeId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LotHistory_Lot_LotId",
                        column: x => x.LotId,
                        principalTable: "Lot",
                        principalColumn: "LotId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "OwnerLot",
                columns: table => new
                {
                    IsArchive = table.Column<bool>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    OwnerLotId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OwnerId = table.Column<int>(nullable: false),
                    LotId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerLot", x => x.OwnerLotId);
                    table.ForeignKey(
                        name: "FK_OwnerLot_Lot_LotId",
                        column: x => x.LotId,
                        principalTable: "Lot",
                        principalColumn: "LotId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OwnerLot_Owner_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owner",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LotHistoryId = table.Column<int>(nullable: true),
                    OwnerHistoryId = table.Column<int>(nullable: true),
                    OwnerId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Private = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comment_LotHistory_LotHistoryId",
                        column: x => x.LotHistoryId,
                        principalTable: "LotHistory",
                        principalColumn: "LotHistoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comment_OwnerHistory_OwnerHistoryId",
                        column: x => x.OwnerHistoryId,
                        principalTable: "OwnerHistory",
                        principalColumn: "OwnerHistoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comment_Owner_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owner",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_LotHistoryId",
                table: "Comment",
                column: "LotHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_OwnerHistoryId",
                table: "Comment",
                column: "OwnerHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_OwnerId",
                table: "Comment",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LotHistory_HistoryTypeId",
                table: "LotHistory",
                column: "HistoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LotHistory_LotId",
                table: "LotHistory",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerLot_LotId",
                table: "OwnerLot",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerLot_OwnerId",
                table: "OwnerLot",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_LotHistory_LotHistoryId",
                table: "File",
                column: "LotHistoryId",
                principalTable: "LotHistory",
                principalColumn: "LotHistoryId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_LotHistory_LotHistoryId",
                table: "File");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "OwnerLot");

            migrationBuilder.DropTable(
                name: "LotHistory");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "OwnerHistory",
                newName: "PrivacyLevel");

            migrationBuilder.RenameColumn(
                name: "Hours",
                table: "OwnerHistory",
                newName: "LotId");

            migrationBuilder.RenameColumn(
                name: "OwnerHistoryId",
                table: "OwnerHistory",
                newName: "LotHistoryId");

            migrationBuilder.AddColumn<int>(
                name: "LogId",
                table: "OwnerHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoOwnerId",
                table: "Owner",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Lot",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OwnerHistory_LotId",
                table: "OwnerHistory",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_Owner_CoOwnerId",
                table: "Owner",
                column: "CoOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Lot_OwnerId",
                table: "Lot",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_OwnerHistory_LotHistoryId",
                table: "File",
                column: "LotHistoryId",
                principalTable: "OwnerHistory",
                principalColumn: "LotHistoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lot_Owner_OwnerId",
                table: "Lot",
                column: "OwnerId",
                principalTable: "Owner",
                principalColumn: "OwnerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Owner_Owner_CoOwnerId",
                table: "Owner",
                column: "CoOwnerId",
                principalTable: "Owner",
                principalColumn: "OwnerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerHistory_Lot_LotId",
                table: "OwnerHistory",
                column: "LotId",
                principalTable: "Lot",
                principalColumn: "LotId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
