using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.ServiceDirectory.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrgOnLocationAndDelReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Organisations_OrganisationId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_OrganisationId",
                table: "Reviews");

            migrationBuilder.AddColumn<long>(
                name: "OrganisationId",
                table: "Locations",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_OrganisationId",
                table: "Locations",
                column: "OrganisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Organisations_OrganisationId",
                table: "Locations",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Organisations_OrganisationId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_OrganisationId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Locations");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrganisationId",
                table: "Reviews",
                column: "OrganisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Organisations_OrganisationId",
                table: "Reviews",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id");
        }
    }
}
