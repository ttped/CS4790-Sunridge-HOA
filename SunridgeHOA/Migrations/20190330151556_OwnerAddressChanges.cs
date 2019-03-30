using Microsoft.EntityFrameworkCore.Migrations;

namespace SunridgeHOA.Migrations
{
    public partial class OwnerAddressChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Lot");

            migrationBuilder.AlterColumn<string>(
                name: "EmergencyContactPhone",
                table: "Owner",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "EmergencyContactName",
                table: "Owner",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Owner",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Owner",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Apartment",
                table: "Address",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Owner");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Owner");

            migrationBuilder.DropColumn(
                name: "Apartment",
                table: "Address");

            migrationBuilder.AlterColumn<string>(
                name: "EmergencyContactPhone",
                table: "Owner",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmergencyContactName",
                table: "Owner",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Lot",
                nullable: true);
        }
    }
}
