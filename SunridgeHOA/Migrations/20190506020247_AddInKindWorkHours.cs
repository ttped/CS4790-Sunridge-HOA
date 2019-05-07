using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SunridgeHOA.Migrations
{
    public partial class AddInKindWorkHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InKindWorkHours",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Hours = table.Column<int>(nullable: false),
                    FormResponseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InKindWorkHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InKindWorkHours_FormResponse_FormResponseId",
                        column: x => x.FormResponseId,
                        principalTable: "FormResponse",
                        principalColumn: "FormResponseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InKindWorkHours_FormResponseId",
                table: "InKindWorkHours",
                column: "FormResponseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InKindWorkHours");
        }
    }
}
