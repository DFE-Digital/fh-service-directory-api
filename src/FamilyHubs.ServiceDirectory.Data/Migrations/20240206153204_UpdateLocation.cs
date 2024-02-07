using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.ServiceDirectory.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocationType",
                table: "Locations",
                newName: "LocationTypeCategory");

            migrationBuilder.AddColumn<string>(
                name: "AddressType",
                table: "Locations",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlternateName",
                table: "Locations",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Attention",
                table: "Locations",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalIdentifier",
                table: "Locations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalIdentifierType",
                table: "Locations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationType",
                table: "Locations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Locations",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Transportation",
                table: "Locations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Locations",
                type: "nvarchar(2083)",
                maxLength: 2083,
                nullable: true);

            migrationBuilder.Sql(@"UPDATE dbo.Locations SET LocationType = 'Postal'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.DropColumn(
                name: "AddressType",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "AlternateName",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Attention",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ExternalIdentifier",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ExternalIdentifierType",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LocationType",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Transportation",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "LocationTypeCategory",
                table: "Locations",
                newName: "LocationType");
        }
    }
}
