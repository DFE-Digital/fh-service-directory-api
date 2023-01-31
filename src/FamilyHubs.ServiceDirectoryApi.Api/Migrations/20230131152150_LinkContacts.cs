using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fhservicedirectoryapi.api.Migrations
{
    /// <inheritdoc />
    public partial class LinkContacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Services_ServiceId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_ServiceId",
                table: "Contacts");

            migrationBuilder.CreateTable(
                name: "LinkContacts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ContactId = table.Column<string>(type: "text", nullable: true),
                    LinkId = table.Column<string>(type: "text", nullable: false),
                    LinkType = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<string>(type: "text", nullable: true),
                    OrganisationId = table.Column<string>(type: "text", nullable: true),
                    ServiceAtLocationId = table.Column<string>(type: "text", nullable: true),
                    ServiceId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkContacts_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LinkContacts_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LinkContacts_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LinkContacts_ServiceAtLocations_ServiceAtLocationId",
                        column: x => x.ServiceAtLocationId,
                        principalTable: "ServiceAtLocations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LinkContacts_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LinkContacts_ContactId",
                table: "LinkContacts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkContacts_LocationId",
                table: "LinkContacts",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkContacts_OrganisationId",
                table: "LinkContacts",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkContacts_ServiceAtLocationId",
                table: "LinkContacts",
                column: "ServiceAtLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkContacts_ServiceId",
                table: "LinkContacts",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkContacts");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ServiceId",
                table: "Contacts",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Services_ServiceId",
                table: "Contacts",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
