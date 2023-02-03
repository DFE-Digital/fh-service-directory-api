using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fhservicedirectoryapi.api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLinkContactFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_linkcontacts_locations_locationid",
                table: "linkcontacts");
            migrationBuilder.DropForeignKey(
                name: "fk_linkcontacts_organisations_organisationid",
                table: "linkcontacts");
            migrationBuilder.DropForeignKey(
                name: "fk_linkcontacts_serviceatlocations_serviceatlocationid",
                table: "linkcontacts");
            migrationBuilder.DropForeignKey(
                name: "fk_linkcontacts_services_serviceid",
                table: "linkcontacts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "fk_linkcontacts_locations_locationid",
                column: "LinkId",
                principalTable: "locations",
                principalColumn: "id",
                table: "linkcontacts");
            migrationBuilder.AddForeignKey(
                name: "fk_linkcontacts_organisations_organisationid",
                column: "LinkId",
                principalTable: "organisations",
                principalColumn: "id",
                table: "linkcontacts");
            migrationBuilder.AddForeignKey(
                name: "fk_linkcontacts_serviceatlocations_serviceatlocationid",
                column: "LinkId",
                principalTable: "serviceatlocations",
                principalColumn: "id"
                , table: "linkcontacts");
            migrationBuilder.AddForeignKey(
                name: "fk_linkcontacts_services_serviceid",
                column: "LinkId",
                principalTable: "services",
                principalColumn: "id",
                table: "linkcontacts");
        }
    }
}
