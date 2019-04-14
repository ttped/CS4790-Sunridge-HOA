using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SunridgeHOA.Migrations
{
    public partial class AddedScheduledEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hours",
                table: "OwnerHistory");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyLevel",
                table: "OwnerHistory",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ScheduledEvents",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Subject = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: true),
                    IsFullDay = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledEvents", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduledEvents");

            migrationBuilder.DropColumn(
                name: "PrivacyLevel",
                table: "OwnerHistory");

            migrationBuilder.AddColumn<int>(
                name: "Hours",
                table: "OwnerHistory",
                nullable: true);
        }
    }
}
