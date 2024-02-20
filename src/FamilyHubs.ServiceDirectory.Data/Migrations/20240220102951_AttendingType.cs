using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.ServiceDirectory.Data.Migrations
{
    /// <inheritdoc />
    public partial class AttendingType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendingAccess",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "AttendingType",
                table: "Services");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttendingAccess",
                table: "Services",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AttendingType",
                table: "Services",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
