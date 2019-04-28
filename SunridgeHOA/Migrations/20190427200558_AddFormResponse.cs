using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SunridgeHOA.Migrations
{
    public partial class AddFormResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Comment_FormHistory_FormHistoryId",
            //    table: "Comment");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_File_FormHistory_FormHistoryId",
            //    table: "File");

            //migrationBuilder.DropTable(
            //    name: "FormHistory");

            //migrationBuilder.DropIndex(
            //    name: "IX_File_FormHistoryId",
            //    table: "File");

            //migrationBuilder.DropIndex(
            //    name: "IX_Comment_FormHistoryId",
            //    table: "Comment");

            //migrationBuilder.RenameColumn(
            //    name: "FormHistoryId",
            //    table: "Comment",
            //    newName: "FormResponseIdId");

            migrationBuilder.AddColumn<int>(
                name: "FormResponseId",
                table: "File",
                nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "OwnerHistoryId",
            //    table: "File",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "FormResponseId",
            //    table: "Comment",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "FormResponse",
                columns: table => new
                {
                    FormResponseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OwnerId = table.Column<int>(nullable: false),
                    FormType = table.Column<string>(maxLength: 3, nullable: false),
                    LotId = table.Column<int>(nullable: true),
                    SubmitDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Suggestion = table.Column<string>(nullable: true),
                    PrivacyLevel = table.Column<string>(nullable: true),
                    Resolved = table.Column<bool>(nullable: false),
                    ResolveDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormResponse", x => x.FormResponseId);
                    table.ForeignKey(
                        name: "FK_FormResponse_Lot_LotId",
                        column: x => x.LotId,
                        principalTable: "Lot",
                        principalColumn: "LotId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormResponse_Owner_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owner",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LotHistoryId = table.Column<int>(nullable: true),
                    FormResponseIdId = table.Column<int>(nullable: true),
                    OwnerId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Private = table.Column<bool>(nullable: false),
                    FormResponseId = table.Column<int>(nullable: true),
                    OwnerHistoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comment_FormResponse_FormResponseId",
                        column: x => x.FormResponseId,
                        principalTable: "FormResponse",
                        principalColumn: "FormResponseId",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "OwnerHistory",
            //    columns: table => new
            //    {
            //        IsArchive = table.Column<bool>(nullable: false),
            //        LastModifiedBy = table.Column<string>(nullable: true),
            //        LastModifiedDate = table.Column<DateTime>(nullable: false),
            //        OwnerHistoryId = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        OwnerId = table.Column<int>(nullable: false),
            //        HistoryTypeId = table.Column<int>(nullable: false),
            //        Date = table.Column<DateTime>(nullable: false),
            //        Description = table.Column<string>(nullable: true),
            //        PrivacyLevel = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_OwnerHistory", x => x.OwnerHistoryId);
            //        table.ForeignKey(
            //            name: "FK_OwnerHistory_HistoryType_HistoryTypeId",
            //            column: x => x.HistoryTypeId,
            //            principalTable: "HistoryType",
            //            principalColumn: "HistoryTypeId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_OwnerHistory_Owner_OwnerId",
            //            column: x => x.OwnerId,
            //            principalTable: "Owner",
            //            principalColumn: "OwnerId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_File_FormResponseId",
                table: "File",
                column: "FormResponseId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_File_OwnerHistoryId",
            //    table: "File",
            //    column: "OwnerHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_FormResponseId",
                table: "Comment",
                column: "FormResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_OwnerHistoryId",
                table: "Comment",
                column: "OwnerHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FormResponse_LotId",
                table: "FormResponse",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_FormResponse_OwnerId",
                table: "FormResponse",
                column: "OwnerId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_OwnerHistory_HistoryTypeId",
            //    table: "OwnerHistory",
            //    column: "HistoryTypeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_OwnerHistory_OwnerId",
            //    table: "OwnerHistory",
            //    column: "OwnerId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Comment_FormResponse_FormResponseId",
            //    table: "Comment",
            //    column: "FormResponseId",
            //    principalTable: "FormResponse",
            //    principalColumn: "FormResponseId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Comment_OwnerHistory_OwnerHistoryId",
            //    table: "Comment",
            //    column: "OwnerHistoryId",
            //    principalTable: "OwnerHistory",
            //    principalColumn: "OwnerHistoryId",
            //    onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_File_FormResponse_FormResponseId",
                table: "File",
                column: "FormResponseId",
                principalTable: "FormResponse",
                principalColumn: "FormResponseId",
                onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_File_OwnerHistory_OwnerHistoryId",
            //    table: "File",
            //    column: "OwnerHistoryId",
            //    principalTable: "OwnerHistory",
            //    principalColumn: "OwnerHistoryId",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        //    migrationBuilder.DropForeignKey(
        //        name: "FK_Comment_FormResponse_FormResponseId",
        //        table: "Comment");

        //    migrationBuilder.DropForeignKey(
        //        name: "FK_Comment_OwnerHistory_OwnerHistoryId",
        //        table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_File_FormResponse_FormResponseId",
                table: "File");

            migrationBuilder.DropForeignKey(
                name: "FK_File_OwnerHistory_OwnerHistoryId",
                table: "File");

            migrationBuilder.DropTable(
                name: "FormResponse");

            migrationBuilder.DropTable(
                name: "OwnerHistory");

            migrationBuilder.DropIndex(
                name: "IX_File_FormResponseId",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_File_OwnerHistoryId",
                table: "File");

            //migrationBuilder.DropIndex(
            //    name: "IX_Comment_FormResponseId",
            //    table: "Comment");

            //migrationBuilder.DropIndex(
            //    name: "IX_Comment_OwnerHistoryId",
            //    table: "Comment");

            migrationBuilder.DropColumn(
                name: "FormResponseId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "OwnerHistoryId",
                table: "File");

            //migrationBuilder.DropColumn(
            //    name: "FormResponseId",
            //    table: "Comment");

            //migrationBuilder.RenameColumn(
            //    name: "FormResponseIdId",
            //    table: "Comment",
            //    newName: "FormHistoryId");

            migrationBuilder.CreateTable(
                name: "FormHistory",
                columns: table => new
                {
                    FormHistoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    HistoryTypeId = table.Column<int>(nullable: false),
                    IsArchive = table.Column<bool>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LotId = table.Column<int>(nullable: true),
                    OwnerId = table.Column<int>(nullable: false),
                    PrivacyLevel = table.Column<string>(nullable: true),
                    Resolved = table.Column<bool>(nullable: false),
                    SubmitDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormHistory", x => x.FormHistoryId);
                    table.ForeignKey(
                        name: "FK_FormHistory_HistoryType_HistoryTypeId",
                        column: x => x.HistoryTypeId,
                        principalTable: "HistoryType",
                        principalColumn: "HistoryTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormHistory_Lot_LotId",
                        column: x => x.LotId,
                        principalTable: "Lot",
                        principalColumn: "LotId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormHistory_Owner_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owner",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_FormHistoryId",
                table: "File",
                column: "FormHistoryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Comment_FormHistoryId",
            //    table: "Comment",
            //    column: "FormHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FormHistory_HistoryTypeId",
                table: "FormHistory",
                column: "HistoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FormHistory_LotId",
                table: "FormHistory",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_FormHistory_OwnerId",
                table: "FormHistory",
                column: "OwnerId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Comment_FormHistory_FormHistoryId",
            //    table: "Comment",
            //    column: "FormHistoryId",
            //    principalTable: "FormHistory",
            //    principalColumn: "FormHistoryId",
            //    onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_File_FormHistory_FormHistoryId",
                table: "File",
                column: "FormHistoryId",
                principalTable: "FormHistory",
                principalColumn: "FormHistoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
