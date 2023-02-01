using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fhservicedirectoryapi.api.Migrations
{
    /// <inheritdoc />
    public partial class LinkContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accessibility_For_Disabilities");

            migrationBuilder.DropTable(
                name: "OpenReferralContacts");

            migrationBuilder.DropTable(
                name: "OpenReferralCost_Options");

            migrationBuilder.DropTable(
                name: "OpenReferralFundings");

            migrationBuilder.DropTable(
                name: "OpenReferralHoliday_Schedules");

            migrationBuilder.DropTable(
                name: "OpenReferralLanguages");

            migrationBuilder.DropTable(
                name: "OpenReferralLinkTaxonomies");

            migrationBuilder.DropTable(
                name: "OpenReferralPhysical_Addresses");

            migrationBuilder.DropTable(
                name: "OpenReferralRegular_Schedules");

            migrationBuilder.DropTable(
                name: "OpenReferralReviews");

            migrationBuilder.DropTable(
                name: "OpenReferralService_Areas");

            migrationBuilder.DropTable(
                name: "OpenReferralService_Taxonomies");

            migrationBuilder.DropTable(
                name: "OpenReferralServiceDeliveries");

            migrationBuilder.DropTable(
                name: "OpenReferralServiceAtLocations");

            migrationBuilder.DropTable(
                name: "OpenReferralParents");

            migrationBuilder.DropTable(
                name: "OpenReferralTaxonomies");

            migrationBuilder.DropTable(
                name: "OpenReferralLocations");

            migrationBuilder.DropTable(
                name: "OpenReferralEligibilities");

            migrationBuilder.DropTable(
                name: "OpenReferralServices");

            migrationBuilder.DropTable(
                name: "OpenReferralOrganisations");

            migrationBuilder.RenameColumn(
                name: "OpenReferralOrganisationId",
                table: "AdminAreas",
                newName: "OrganisationId");

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Telephone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TextPhone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organisations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    OrganisationTypeId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Logo = table.Column<string>(type: "text", nullable: true),
                    Uri = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organisations_OrganisationTypes_OrganisationTypeId",
                        column: x => x.OrganisationTypeId,
                        principalTable: "OrganisationTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Vocabulary = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccessibilityForDisabilities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Accessibility = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessibilityForDisabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessibilityForDisabilities_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalAddresses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<string>(type: "text", nullable: false),
                    Address1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PostCode = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Country = table.Column<string>(type: "text", nullable: true),
                    StateProvince = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalAddresses_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ServiceTypeId = table.Column<string>(type: "text", nullable: true),
                    OrganisationId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Accreditations = table.Column<string>(type: "text", nullable: true),
                    AssuredDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AttendingAccess = table.Column<string>(type: "text", nullable: true),
                    AttendingType = table.Column<string>(type: "text", nullable: true),
                    DeliverableType = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Fees = table.Column<string>(type: "text", nullable: true),
                    CanFamilyChooseDeliveryLocation = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CostOptions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    AmountDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    LinkId = table.Column<string>(type: "text", nullable: true),
                    Option = table.Column<string>(type: "text", nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ServiceId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostOptions_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Eligibilities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    EligibilityDescription = table.Column<string>(type: "text", nullable: false),
                    LinkId = table.Column<string>(type: "text", nullable: true),
                    MaximumAge = table.Column<int>(type: "integer", nullable: false),
                    MinimumAge = table.Column<int>(type: "integer", nullable: false),
                    ServiceId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eligibilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eligibilities_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fundings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    ServiceId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fundings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fundings_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ServiceId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Languages_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Score = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true),
                    Widget = table.Column<string>(type: "text", nullable: true),
                    ServiceId = table.Column<string>(type: "text", nullable: false),
                    OrganisationId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "ServiceAreas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ServiceAreaDescription = table.Column<string>(type: "text", nullable: false),
                    LinkId = table.Column<string>(type: "text", nullable: true),
                    Extent = table.Column<string>(type: "text", nullable: true),
                    Uri = table.Column<string>(type: "text", nullable: true),
                    ServiceId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceAreas_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceAtLocations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<string>(type: "text", nullable: false),
                    ServiceId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceAtLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceAtLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceAtLocations_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceDeliveries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ServiceId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceDeliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceDeliveries_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Taxonomies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Vocabulary = table.Column<string>(type: "text", nullable: true),
                    Parent = table.Column<string>(type: "text", nullable: true),
                    EligibilityId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxonomies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Taxonomies_Eligibilities_EligibilityId",
                        column: x => x.EligibilityId,
                        principalTable: "Eligibilities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HolidaySchedules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Closed = table.Column<bool>(type: "boolean", nullable: false),
                    ClosesAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OpensAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ServiceAtLocationId = table.Column<string>(type: "text", nullable: false),
                    ServiceId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidaySchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolidaySchedules_ServiceAtLocations_ServiceAtLocationId",
                        column: x => x.ServiceAtLocationId,
                        principalTable: "ServiceAtLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HolidaySchedules_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LinkContacts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ContactId = table.Column<string>(type: "text", nullable: true),
                    LinkId = table.Column<string>(type: "text", nullable: false),
                    LinkType = table.Column<string>(type: "text", nullable: false),
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
                        name: "FK_LinkContacts_Locations_LinkId",
                        column: x => x.LinkId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkContacts_Organisations_LinkId",
                        column: x => x.LinkId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkContacts_ServiceAtLocations_LinkId",
                        column: x => x.LinkId,
                        principalTable: "ServiceAtLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkContacts_Services_LinkId",
                        column: x => x.LinkId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegularSchedules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    OpensAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClosesAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ByDay = table.Column<string>(type: "text", nullable: true),
                    ByMonthDay = table.Column<string>(type: "text", nullable: true),
                    DtStart = table.Column<string>(type: "text", nullable: true),
                    Freq = table.Column<string>(type: "text", nullable: true),
                    Interval = table.Column<string>(type: "text", nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ServiceAtLocationId = table.Column<string>(type: "text", nullable: false),
                    ServiceId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegularSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegularSchedules_ServiceAtLocations_ServiceAtLocationId",
                        column: x => x.ServiceAtLocationId,
                        principalTable: "ServiceAtLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegularSchedules_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LinkTaxonomies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TaxonomyId = table.Column<string>(type: "text", nullable: true),
                    LinkId = table.Column<string>(type: "text", nullable: false),
                    LinkType = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkTaxonomies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkTaxonomies_Locations_LinkId",
                        column: x => x.LinkId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkTaxonomies_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LinkTaxonomies_Taxonomies_TaxonomyId",
                        column: x => x.TaxonomyId,
                        principalTable: "Taxonomies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceTaxonomies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    LinkId = table.Column<string>(type: "text", nullable: true),
                    TaxonomyId = table.Column<string>(type: "text", nullable: true),
                    ServiceId = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTaxonomies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceTaxonomies_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceTaxonomies_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceTaxonomies_Taxonomies_TaxonomyId",
                        column: x => x.TaxonomyId,
                        principalTable: "Taxonomies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessibilityForDisabilities_LocationId",
                table: "AccessibilityForDisabilities",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostOptions_ServiceId",
                table: "CostOptions",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Eligibilities_ServiceId",
                table: "Eligibilities",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Fundings_ServiceId",
                table: "Fundings",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_HolidaySchedules_ServiceAtLocationId",
                table: "HolidaySchedules",
                column: "ServiceAtLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_HolidaySchedules_ServiceId",
                table: "HolidaySchedules",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_ServiceId",
                table: "Languages",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkContacts_ContactId",
                table: "LinkContacts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkContacts_LinkId",
                table: "LinkContacts",
                column: "LinkId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkTaxonomies_LinkId",
                table: "LinkTaxonomies",
                column: "LinkId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkTaxonomies_ParentId",
                table: "LinkTaxonomies",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkTaxonomies_TaxonomyId",
                table: "LinkTaxonomies",
                column: "TaxonomyId");

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_OrganisationTypeId",
                table: "Organisations",
                column: "OrganisationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalAddresses_LocationId",
                table: "PhysicalAddresses",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_RegularSchedules_ServiceAtLocationId",
                table: "RegularSchedules",
                column: "ServiceAtLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_RegularSchedules_ServiceId",
                table: "RegularSchedules",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrganisationId",
                table: "Reviews",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ServiceId",
                table: "Reviews",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAreas_ServiceId",
                table: "ServiceAreas",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAtLocations_LocationId",
                table: "ServiceAtLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAtLocations_ServiceId",
                table: "ServiceAtLocations",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceDeliveries_ServiceId",
                table: "ServiceDeliveries",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_OrganisationId",
                table: "Services",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTaxonomies_ParentId",
                table: "ServiceTaxonomies",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTaxonomies_ServiceId",
                table: "ServiceTaxonomies",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTaxonomies_TaxonomyId",
                table: "ServiceTaxonomies",
                column: "TaxonomyId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxonomies_EligibilityId",
                table: "Taxonomies",
                column: "EligibilityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessibilityForDisabilities");

            migrationBuilder.DropTable(
                name: "CostOptions");

            migrationBuilder.DropTable(
                name: "Fundings");

            migrationBuilder.DropTable(
                name: "HolidaySchedules");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "LinkContacts");

            migrationBuilder.DropTable(
                name: "LinkTaxonomies");

            migrationBuilder.DropTable(
                name: "PhysicalAddresses");

            migrationBuilder.DropTable(
                name: "RegularSchedules");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "ServiceAreas");

            migrationBuilder.DropTable(
                name: "ServiceDeliveries");

            migrationBuilder.DropTable(
                name: "ServiceTaxonomies");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "ServiceAtLocations");

            migrationBuilder.DropTable(
                name: "Parents");

            migrationBuilder.DropTable(
                name: "Taxonomies");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Eligibilities");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Organisations");

            migrationBuilder.RenameColumn(
                name: "OrganisationId",
                table: "AdminAreas",
                newName: "OpenReferralOrganisationId");

            migrationBuilder.CreateTable(
                name: "OpenReferralLocations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralOrganisations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    OrganisationTypeId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Logo = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Uri = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralOrganisations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralOrganisations_OrganisationTypes_OrganisationTyp~",
                        column: x => x.OrganisationTypeId,
                        principalTable: "OrganisationTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralParents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Vocabulary = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralParents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accessibility_For_Disabilities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Accessibility = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    OpenReferralLocationId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accessibility_For_Disabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accessibility_For_Disabilities_OpenReferralLocations_OpenRe~",
                        column: x => x.OpenReferralLocationId,
                        principalTable: "OpenReferralLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralPhysical_Addresses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Address1 = table.Column<string>(name: "Address_1", type: "character varying(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    OpenReferralLocationId = table.Column<string>(type: "text", nullable: false),
                    Postalcode = table.Column<string>(name: "Postal_code", type: "character varying(15)", maxLength: 15, nullable: false),
                    Stateprovince = table.Column<string>(name: "State_province", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralPhysical_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralPhysical_Addresses_OpenReferralLocations_OpenRe~",
                        column: x => x.OpenReferralLocationId,
                        principalTable: "OpenReferralLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralServices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ServiceTypeId = table.Column<string>(type: "text", nullable: true),
                    Accreditations = table.Column<string>(type: "text", nullable: true),
                    Assureddate = table.Column<DateTime>(name: "Assured_date", type: "timestamp with time zone", nullable: true),
                    Attendingaccess = table.Column<string>(name: "Attending_access", type: "text", nullable: true),
                    Attendingtype = table.Column<string>(name: "Attending_type", type: "text", nullable: true),
                    CanFamilyChooseDeliveryLocation = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Deliverabletype = table.Column<string>(name: "Deliverable_type", type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Fees = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    OpenReferralOrganisationId = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralServices_OpenReferralOrganisations_OpenReferral~",
                        column: x => x.OpenReferralOrganisationId,
                        principalTable: "OpenReferralOrganisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpenReferralServices_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralContacts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false),
                    Telephone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TextPhone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralContacts_OpenReferralServices_OpenReferralServi~",
                        column: x => x.OpenReferralServiceId,
                        principalTable: "OpenReferralServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralCost_Options",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Amountdescription = table.Column<string>(name: "Amount_description", type: "character varying(500)", maxLength: 500, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LinkId = table.Column<string>(type: "text", nullable: true),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false),
                    Option = table.Column<string>(type: "text", nullable: true),
                    Validfrom = table.Column<DateTime>(name: "Valid_from", type: "timestamp with time zone", nullable: true),
                    Validto = table.Column<DateTime>(name: "Valid_to", type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralCost_Options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralCost_Options_OpenReferralServices_OpenReferralS~",
                        column: x => x.OpenReferralServiceId,
                        principalTable: "OpenReferralServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralEligibilities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Eligibility = table.Column<string>(type: "text", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LinkId = table.Column<string>(type: "text", nullable: true),
                    Maximumage = table.Column<int>(name: "Maximum_age", type: "integer", nullable: false),
                    Minimumage = table.Column<int>(name: "Minimum_age", type: "integer", nullable: false),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralEligibilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralEligibilities_OpenReferralServices_OpenReferral~",
                        column: x => x.OpenReferralServiceId,
                        principalTable: "OpenReferralServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralFundings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: true),
                    Source = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralFundings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralFundings_OpenReferralServices_OpenReferralServi~",
                        column: x => x.OpenReferralServiceId,
                        principalTable: "OpenReferralServices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralLanguages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Language = table.Column<string>(type: "text", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralLanguages_OpenReferralServices_OpenReferralServ~",
                        column: x => x.OpenReferralServiceId,
                        principalTable: "OpenReferralServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralReviews",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    OpenReferralOrganisationId = table.Column<string>(type: "text", nullable: true),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false),
                    Score = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: true),
                    Widget = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralReviews_OpenReferralOrganisations_OpenReferralO~",
                        column: x => x.OpenReferralOrganisationId,
                        principalTable: "OpenReferralOrganisations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OpenReferralReviews_OpenReferralServices_OpenReferralServic~",
                        column: x => x.OpenReferralServiceId,
                        principalTable: "OpenReferralServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralService_Areas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Extent = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LinkId = table.Column<string>(type: "text", nullable: true),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false),
                    Servicearea = table.Column<string>(name: "Service_area", type: "text", nullable: false),
                    Uri = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralService_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralService_Areas_OpenReferralServices_OpenReferral~",
                        column: x => x.OpenReferralServiceId,
                        principalTable: "OpenReferralServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralServiceAtLocations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralServiceAtLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralServiceAtLocations_OpenReferralLocations_Locati~",
                        column: x => x.LocationId,
                        principalTable: "OpenReferralLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpenReferralServiceAtLocations_OpenReferralServices_OpenRef~",
                        column: x => x.OpenReferralServiceId,
                        principalTable: "OpenReferralServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralServiceDeliveries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false),
                    ServiceDelivery = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralServiceDeliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralServiceDeliveries_OpenReferralServices_OpenRefe~",
                        column: x => x.OpenReferralServiceId,
                        principalTable: "OpenReferralServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralTaxonomies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    OpenReferralEligibilityId = table.Column<string>(type: "text", nullable: true),
                    Parent = table.Column<string>(type: "text", nullable: true),
                    Vocabulary = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralTaxonomies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralTaxonomies_OpenReferralEligibilities_OpenReferr~",
                        column: x => x.OpenReferralEligibilityId,
                        principalTable: "OpenReferralEligibilities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralHoliday_Schedules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Closed = table.Column<bool>(type: "boolean", nullable: false),
                    Closesat = table.Column<DateTime>(name: "Closes_at", type: "timestamp with time zone", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Enddate = table.Column<DateTime>(name: "End_date", type: "timestamp with time zone", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    OpenReferralServiceAtLocationId = table.Column<string>(type: "text", nullable: false),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: true),
                    Opensat = table.Column<DateTime>(name: "Opens_at", type: "timestamp with time zone", nullable: true),
                    Startdate = table.Column<DateTime>(name: "Start_date", type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralHoliday_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralHoliday_Schedules_OpenReferralServiceAtLocation~",
                        column: x => x.OpenReferralServiceAtLocationId,
                        principalTable: "OpenReferralServiceAtLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpenReferralHoliday_Schedules_OpenReferralServices_OpenRefe~",
                        column: x => x.OpenReferralServiceId,
                        principalTable: "OpenReferralServices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralRegular_Schedules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Byday = table.Column<string>(type: "text", nullable: true),
                    Bymonthday = table.Column<string>(type: "text", nullable: true),
                    Closesat = table.Column<DateTime>(name: "Closes_at", type: "timestamp with time zone", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Dtstart = table.Column<string>(type: "text", nullable: true),
                    Freq = table.Column<string>(type: "text", nullable: true),
                    Interval = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    OpenReferralServiceAtLocationId = table.Column<string>(type: "text", nullable: false),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: true),
                    Opensat = table.Column<DateTime>(name: "Opens_at", type: "timestamp with time zone", nullable: true),
                    Validfrom = table.Column<DateTime>(name: "Valid_from", type: "timestamp with time zone", nullable: true),
                    Validto = table.Column<DateTime>(name: "Valid_to", type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralRegular_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralRegular_Schedules_OpenReferralServiceAtLocation~",
                        column: x => x.OpenReferralServiceAtLocationId,
                        principalTable: "OpenReferralServiceAtLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpenReferralRegular_Schedules_OpenReferralServices_OpenRefe~",
                        column: x => x.OpenReferralServiceId,
                        principalTable: "OpenReferralServices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralLinkTaxonomies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TaxonomyId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LinkId = table.Column<string>(type: "text", nullable: false),
                    LinkType = table.Column<string>(type: "text", nullable: false),
                    OpenReferralLocationId = table.Column<string>(type: "text", nullable: true),
                    OpenReferralParentId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralLinkTaxonomies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralLinkTaxonomies_OpenReferralLocations_OpenReferr~",
                        column: x => x.OpenReferralLocationId,
                        principalTable: "OpenReferralLocations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OpenReferralLinkTaxonomies_OpenReferralParents_OpenReferral~",
                        column: x => x.OpenReferralParentId,
                        principalTable: "OpenReferralParents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OpenReferralLinkTaxonomies_OpenReferralTaxonomies_TaxonomyId",
                        column: x => x.TaxonomyId,
                        principalTable: "OpenReferralTaxonomies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralService_Taxonomies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TaxonomyId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LinkId = table.Column<string>(type: "text", nullable: true),
                    OpenReferralParentId = table.Column<string>(type: "text", nullable: true),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralService_Taxonomies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralService_Taxonomies_OpenReferralParents_OpenRefe~",
                        column: x => x.OpenReferralParentId,
                        principalTable: "OpenReferralParents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OpenReferralService_Taxonomies_OpenReferralServices_OpenRef~",
                        column: x => x.OpenReferralServiceId,
                        principalTable: "OpenReferralServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpenReferralService_Taxonomies_OpenReferralTaxonomies_Taxon~",
                        column: x => x.TaxonomyId,
                        principalTable: "OpenReferralTaxonomies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accessibility_For_Disabilities_OpenReferralLocationId",
                table: "Accessibility_For_Disabilities",
                column: "OpenReferralLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralContacts_OpenReferralServiceId",
                table: "OpenReferralContacts",
                column: "OpenReferralServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralCost_Options_OpenReferralServiceId",
                table: "OpenReferralCost_Options",
                column: "OpenReferralServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralEligibilities_OpenReferralServiceId",
                table: "OpenReferralEligibilities",
                column: "OpenReferralServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralFundings_OpenReferralServiceId",
                table: "OpenReferralFundings",
                column: "OpenReferralServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralHoliday_Schedules_OpenReferralServiceAtLocation~",
                table: "OpenReferralHoliday_Schedules",
                column: "OpenReferralServiceAtLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralHoliday_Schedules_OpenReferralServiceId",
                table: "OpenReferralHoliday_Schedules",
                column: "OpenReferralServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralLanguages_OpenReferralServiceId",
                table: "OpenReferralLanguages",
                column: "OpenReferralServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralLinkTaxonomies_OpenReferralLocationId",
                table: "OpenReferralLinkTaxonomies",
                column: "OpenReferralLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralLinkTaxonomies_OpenReferralParentId",
                table: "OpenReferralLinkTaxonomies",
                column: "OpenReferralParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralLinkTaxonomies_TaxonomyId",
                table: "OpenReferralLinkTaxonomies",
                column: "TaxonomyId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralOrganisations_OrganisationTypeId",
                table: "OpenReferralOrganisations",
                column: "OrganisationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralPhysical_Addresses_OpenReferralLocationId",
                table: "OpenReferralPhysical_Addresses",
                column: "OpenReferralLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralRegular_Schedules_OpenReferralServiceAtLocation~",
                table: "OpenReferralRegular_Schedules",
                column: "OpenReferralServiceAtLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralRegular_Schedules_OpenReferralServiceId",
                table: "OpenReferralRegular_Schedules",
                column: "OpenReferralServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralReviews_OpenReferralOrganisationId",
                table: "OpenReferralReviews",
                column: "OpenReferralOrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralReviews_OpenReferralServiceId",
                table: "OpenReferralReviews",
                column: "OpenReferralServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralService_Areas_OpenReferralServiceId",
                table: "OpenReferralService_Areas",
                column: "OpenReferralServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralService_Taxonomies_OpenReferralParentId",
                table: "OpenReferralService_Taxonomies",
                column: "OpenReferralParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralService_Taxonomies_OpenReferralServiceId",
                table: "OpenReferralService_Taxonomies",
                column: "OpenReferralServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralService_Taxonomies_TaxonomyId",
                table: "OpenReferralService_Taxonomies",
                column: "TaxonomyId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralServiceAtLocations_LocationId",
                table: "OpenReferralServiceAtLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralServiceAtLocations_OpenReferralServiceId",
                table: "OpenReferralServiceAtLocations",
                column: "OpenReferralServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralServiceDeliveries_OpenReferralServiceId",
                table: "OpenReferralServiceDeliveries",
                column: "OpenReferralServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralServices_OpenReferralOrganisationId",
                table: "OpenReferralServices",
                column: "OpenReferralOrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralServices_ServiceTypeId",
                table: "OpenReferralServices",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralTaxonomies_OpenReferralEligibilityId",
                table: "OpenReferralTaxonomies",
                column: "OpenReferralEligibilityId");
        }
    }
}
