using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fhservicedirectoryapi.api.Migrations
{
    /// <inheritdoc />
    public partial class TaxonomyTypeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "vocabulary",
                table: "taxonomies");

            migrationBuilder.AddColumn<string>(
                name: "taxonomytype",
                table: "taxonomies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "address1",
                table: "physicaladdresses",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "taxonomytype",
                table: "taxonomies");

            migrationBuilder.AddColumn<string>(
                name: "vocabulary",
                table: "taxonomies",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "address1",
                table: "physicaladdresses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024);
        }
    }
}
