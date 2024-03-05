using System;
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
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.AddColumn<long>(
                name: "OrganisationId",
                table: "Locations",
                type: "bigint",
                nullable: true);

            // Where a location isn't currently associated with an organisation, and the organisation has services:
            //    set the org's location to one of the service's locations.
            // If a location isn't currently associated with an organisation, and the organisation has no services:
            //    don't associate an organisation with the location.
            // In prod, all locations are associated with a service.
            // In test, we have some test locations that haven't been associated with a service.
            // If a location is already associated with an organisation, we don't change it.
            // That's so that if this migration is removed and then reapplied post release,
            // where new locations have been added (and associated with an organisation),
            // we don't overwrite the existing organisation association.

            migrationBuilder.Sql(
                @"UPDATE Locations
            SET OrganisationId = Services.OrganisationId
            FROM Locations
            INNER JOIN ServiceAtLocations ON Locations.Id = ServiceAtLocations.LocationId
            INNER JOIN Services ON ServiceAtLocations.ServiceId = Services.Id
            WHERE Locations.OrganisationId IS NULL;");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_OrganisationId",
                table: "Locations",
                column: "OrganisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Organisations_OrganisationId",
                table: "Locations",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    OrganisationId = table.Column<long>(type: "bigint", nullable: true),
                    Score = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ServiceId = table.Column<long>(type: "bigint", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: true),
                    Widget = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviews_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrganisationId",
                table: "Reviews",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ServiceId",
                table: "Reviews",
                column: "ServiceId");
        }
    }
}
