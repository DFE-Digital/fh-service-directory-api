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

            migrationBuilder.DropPrimaryKey(
                name: "PK_UICaches",
                table: "UICaches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceTypes",
                table: "ServiceTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RelatedOrganisations",
                table: "RelatedOrganisations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganisationTypes",
                table: "OrganisationTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminAreas",
                table: "AdminAreas");

            migrationBuilder.RenameTable(
                name: "UICaches",
                newName: "uicaches");

            migrationBuilder.RenameTable(
                name: "ServiceTypes",
                newName: "servicetypes");

            migrationBuilder.RenameTable(
                name: "RelatedOrganisations",
                newName: "relatedorganisations");

            migrationBuilder.RenameTable(
                name: "OrganisationTypes",
                newName: "organisationtypes");

            migrationBuilder.RenameTable(
                name: "AdminAreas",
                newName: "adminareas");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "uicaches",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "uicaches",
                newName: "lastmodifiedby");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "uicaches",
                newName: "lastmodified");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "uicaches",
                newName: "createdby");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "uicaches",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "uicaches",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "servicetypes",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "servicetypes",
                newName: "lastmodifiedby");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "servicetypes",
                newName: "lastmodified");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "servicetypes",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "servicetypes",
                newName: "createdby");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "servicetypes",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "servicetypes",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RelatedOrganisationId",
                table: "relatedorganisations",
                newName: "relatedorganisationid");

            migrationBuilder.RenameColumn(
                name: "OrganisationId",
                table: "relatedorganisations",
                newName: "organisationid");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "relatedorganisations",
                newName: "lastmodifiedby");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "relatedorganisations",
                newName: "lastmodified");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "relatedorganisations",
                newName: "createdby");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "relatedorganisations",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "relatedorganisations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "organisationtypes",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "organisationtypes",
                newName: "lastmodifiedby");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "organisationtypes",
                newName: "lastmodified");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "organisationtypes",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "organisationtypes",
                newName: "createdby");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "organisationtypes",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "organisationtypes",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "adminareas",
                newName: "lastmodifiedby");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "adminareas",
                newName: "lastmodified");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "adminareas",
                newName: "createdby");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "adminareas",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "adminareas",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "adminareas",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "OpenReferralOrganisationId",
                table: "adminareas",
                newName: "organisationid");

            migrationBuilder.AddPrimaryKey(
                name: "pk_uicaches",
                table: "uicaches",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_servicetypes",
                table: "servicetypes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_relatedorganisations",
                table: "relatedorganisations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_organisationtypes",
                table: "organisationtypes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_adminareas",
                table: "adminareas",
                column: "id");

            migrationBuilder.CreateTable(
                name: "contacts",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    telephone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    textphone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    url = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contacts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_locations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "organisations",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    organisationtypeid = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    logo = table.Column<string>(type: "text", nullable: true),
                    uri = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organisations", x => x.id);
                    table.ForeignKey(
                        name: "fk_organisations_organisationtypes_organisationtypeid",
                        column: x => x.organisationtypeid,
                        principalTable: "organisationtypes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "parents",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    vocabulary = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_parents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "accessibilityfordisabilities",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    accessibility = table.Column<string>(type: "text", nullable: false),
                    locationid = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accessibilityfordisabilities", x => x.id);
                    table.ForeignKey(
                        name: "fk_accessibilityfordisabilities_locations_locationid",
                        column: x => x.locationid,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "physicaladdresses",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    locationid = table.Column<string>(type: "text", nullable: false),
                    address1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    postcode = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    country = table.Column<string>(type: "text", nullable: true),
                    stateprovince = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_physicaladdresses", x => x.id);
                    table.ForeignKey(
                        name: "fk_physicaladdresses_locations_locationid",
                        column: x => x.locationid,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    servicetypeid = table.Column<string>(type: "text", nullable: true),
                    organisationid = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    accreditations = table.Column<string>(type: "text", nullable: true),
                    assureddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    attendingaccess = table.Column<string>(type: "text", nullable: true),
                    attendingtype = table.Column<string>(type: "text", nullable: true),
                    deliverabletype = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    fees = table.Column<string>(type: "text", nullable: true),
                    canfamilychoosedeliverylocation = table.Column<bool>(type: "boolean", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_services", x => x.id);
                    table.ForeignKey(
                        name: "fk_services_organisations_organisationid",
                        column: x => x.organisationid,
                        principalTable: "organisations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_services_servicetypes_servicetypeid",
                        column: x => x.servicetypeid,
                        principalTable: "servicetypes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "costoptions",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    amountdescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    linkid = table.Column<string>(type: "text", nullable: true),
                    option = table.Column<string>(type: "text", nullable: true),
                    validfrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    validto = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    serviceid = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_costoptions", x => x.id);
                    table.ForeignKey(
                        name: "fk_costoptions_services_serviceid",
                        column: x => x.serviceid,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "eligibilities",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    eligibilitydescription = table.Column<string>(type: "text", nullable: false),
                    linkid = table.Column<string>(type: "text", nullable: true),
                    maximumage = table.Column<int>(type: "integer", nullable: false),
                    minimumage = table.Column<int>(type: "integer", nullable: false),
                    serviceid = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_eligibilities", x => x.id);
                    table.ForeignKey(
                        name: "fk_eligibilities_services_serviceid",
                        column: x => x.serviceid,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fundings",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    source = table.Column<string>(type: "text", nullable: false),
                    serviceid = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fundings", x => x.id);
                    table.ForeignKey(
                        name: "fk_fundings_services_serviceid",
                        column: x => x.serviceid,
                        principalTable: "services",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "languages",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    serviceid = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_languages", x => x.id);
                    table.ForeignKey(
                        name: "fk_languages_services_serviceid",
                        column: x => x.serviceid,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    score = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    widget = table.Column<string>(type: "text", nullable: true),
                    serviceid = table.Column<string>(type: "text", nullable: false),
                    organisationid = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reviews", x => x.id);
                    table.ForeignKey(
                        name: "fk_reviews_organisations_organisationid",
                        column: x => x.organisationid,
                        principalTable: "organisations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_reviews_services_serviceid",
                        column: x => x.serviceid,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "serviceareas",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    serviceareadescription = table.Column<string>(type: "text", nullable: false),
                    linkid = table.Column<string>(type: "text", nullable: true),
                    extent = table.Column<string>(type: "text", nullable: true),
                    uri = table.Column<string>(type: "text", nullable: true),
                    serviceid = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_serviceareas", x => x.id);
                    table.ForeignKey(
                        name: "fk_serviceareas_services_serviceid",
                        column: x => x.serviceid,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "serviceatlocations",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    locationid = table.Column<string>(type: "text", nullable: false),
                    serviceid = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_serviceatlocations", x => x.id);
                    table.ForeignKey(
                        name: "fk_serviceatlocations_locations_locationid",
                        column: x => x.locationid,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_serviceatlocations_services_serviceid",
                        column: x => x.serviceid,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicedeliveries",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    serviceid = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_servicedeliveries", x => x.id);
                    table.ForeignKey(
                        name: "fk_servicedeliveries_services_serviceid",
                        column: x => x.serviceid,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "taxonomies",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    vocabulary = table.Column<string>(type: "text", nullable: true),
                    parent = table.Column<string>(type: "text", nullable: true),
                    eligibilityid = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_taxonomies", x => x.id);
                    table.ForeignKey(
                        name: "fk_taxonomies_eligibilities_eligibilityid",
                        column: x => x.eligibilityid,
                        principalTable: "eligibilities",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "holidayschedules",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    closed = table.Column<bool>(type: "boolean", nullable: false),
                    closesat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    startdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    enddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    opensat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    serviceatlocationid = table.Column<string>(type: "text", nullable: false),
                    serviceid = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_holidayschedules", x => x.id);
                    table.ForeignKey(
                        name: "fk_holidayschedules_serviceatlocations_serviceatlocationid",
                        column: x => x.serviceatlocationid,
                        principalTable: "serviceatlocations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_holidayschedules_services_serviceid",
                        column: x => x.serviceid,
                        principalTable: "services",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "linkcontacts",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    contactid = table.Column<string>(type: "text", nullable: true),
                    linkid = table.Column<string>(type: "text", nullable: false),
                    linktype = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_linkcontacts", x => x.id);
                    table.ForeignKey(
                        name: "fk_linkcontacts_contacts_contactid",
                        column: x => x.contactid,
                        principalTable: "contacts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_linkcontacts_locations_locationid",
                        column: x => x.linkid,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_linkcontacts_organisations_organisationid",
                        column: x => x.linkid,
                        principalTable: "organisations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_linkcontacts_serviceatlocations_serviceatlocationid",
                        column: x => x.linkid,
                        principalTable: "serviceatlocations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_linkcontacts_services_serviceid",
                        column: x => x.linkid,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "regularschedules",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    opensat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    closesat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    byday = table.Column<string>(type: "text", nullable: true),
                    bymonthday = table.Column<string>(type: "text", nullable: true),
                    dtstart = table.Column<string>(type: "text", nullable: true),
                    freq = table.Column<string>(type: "text", nullable: true),
                    interval = table.Column<string>(type: "text", nullable: true),
                    validfrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    validto = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    serviceatlocationid = table.Column<string>(type: "text", nullable: false),
                    serviceid = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_regularschedules", x => x.id);
                    table.ForeignKey(
                        name: "fk_regularschedules_serviceatlocations_serviceatlocationid",
                        column: x => x.serviceatlocationid,
                        principalTable: "serviceatlocations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_regularschedules_services_serviceid",
                        column: x => x.serviceid,
                        principalTable: "services",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "linktaxonomies",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    taxonomyid = table.Column<string>(type: "text", nullable: true),
                    linkid = table.Column<string>(type: "text", nullable: false),
                    linktype = table.Column<string>(type: "text", nullable: false),
                    parentid = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_linktaxonomies", x => x.id);
                    table.ForeignKey(
                        name: "fk_linktaxonomies_locations_locationid",
                        column: x => x.linkid,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_linktaxonomies_parents_parentid",
                        column: x => x.parentid,
                        principalTable: "parents",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_linktaxonomies_taxonomies_taxonomyid",
                        column: x => x.taxonomyid,
                        principalTable: "taxonomies",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "servicetaxonomies",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    linkid = table.Column<string>(type: "text", nullable: true),
                    taxonomyid = table.Column<string>(type: "text", nullable: true),
                    serviceid = table.Column<string>(type: "text", nullable: false),
                    parentid = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_servicetaxonomies", x => x.id);
                    table.ForeignKey(
                        name: "fk_servicetaxonomies_parents_parentid",
                        column: x => x.parentid,
                        principalTable: "parents",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_servicetaxonomies_services_serviceid",
                        column: x => x.serviceid,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_servicetaxonomies_taxonomies_taxonomyid",
                        column: x => x.taxonomyid,
                        principalTable: "taxonomies",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_accessibilityfordisabilities_locationid",
                table: "accessibilityfordisabilities",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "ix_costoptions_serviceid",
                table: "costoptions",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "ix_eligibilities_serviceid",
                table: "eligibilities",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "ix_fundings_serviceid",
                table: "fundings",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "ix_holidayschedules_serviceatlocationid",
                table: "holidayschedules",
                column: "serviceatlocationid");

            migrationBuilder.CreateIndex(
                name: "ix_holidayschedules_serviceid",
                table: "holidayschedules",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "ix_languages_serviceid",
                table: "languages",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "ix_linkcontacts_contactid",
                table: "linkcontacts",
                column: "contactid");

            migrationBuilder.CreateIndex(
                name: "ix_linkcontacts_linkid",
                table: "linkcontacts",
                column: "linkid");

            migrationBuilder.CreateIndex(
                name: "ix_linktaxonomies_linkid",
                table: "linktaxonomies",
                column: "linkid");

            migrationBuilder.CreateIndex(
                name: "ix_linktaxonomies_parentid",
                table: "linktaxonomies",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "ix_linktaxonomies_taxonomyid",
                table: "linktaxonomies",
                column: "taxonomyid");

            migrationBuilder.CreateIndex(
                name: "ix_organisations_organisationtypeid",
                table: "organisations",
                column: "organisationtypeid");

            migrationBuilder.CreateIndex(
                name: "ix_physicaladdresses_locationid",
                table: "physicaladdresses",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "ix_regularschedules_serviceatlocationid",
                table: "regularschedules",
                column: "serviceatlocationid");

            migrationBuilder.CreateIndex(
                name: "ix_regularschedules_serviceid",
                table: "regularschedules",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_organisationid",
                table: "reviews",
                column: "organisationid");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_serviceid",
                table: "reviews",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "ix_serviceareas_serviceid",
                table: "serviceareas",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "ix_serviceatlocations_locationid",
                table: "serviceatlocations",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "ix_serviceatlocations_serviceid",
                table: "serviceatlocations",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "ix_servicedeliveries_serviceid",
                table: "servicedeliveries",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "ix_services_organisationid",
                table: "services",
                column: "organisationid");

            migrationBuilder.CreateIndex(
                name: "ix_services_servicetypeid",
                table: "services",
                column: "servicetypeid");

            migrationBuilder.CreateIndex(
                name: "ix_servicetaxonomies_parentid",
                table: "servicetaxonomies",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "ix_servicetaxonomies_serviceid",
                table: "servicetaxonomies",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "ix_servicetaxonomies_taxonomyid",
                table: "servicetaxonomies",
                column: "taxonomyid");

            migrationBuilder.CreateIndex(
                name: "ix_taxonomies_eligibilityid",
                table: "taxonomies",
                column: "eligibilityid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accessibilityfordisabilities");

            migrationBuilder.DropTable(
                name: "costoptions");

            migrationBuilder.DropTable(
                name: "fundings");

            migrationBuilder.DropTable(
                name: "holidayschedules");

            migrationBuilder.DropTable(
                name: "languages");

            migrationBuilder.DropTable(
                name: "linkcontacts");

            migrationBuilder.DropTable(
                name: "linktaxonomies");

            migrationBuilder.DropTable(
                name: "physicaladdresses");

            migrationBuilder.DropTable(
                name: "regularschedules");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "serviceareas");

            migrationBuilder.DropTable(
                name: "servicedeliveries");

            migrationBuilder.DropTable(
                name: "servicetaxonomies");

            migrationBuilder.DropTable(
                name: "contacts");

            migrationBuilder.DropTable(
                name: "serviceatlocations");

            migrationBuilder.DropTable(
                name: "parents");

            migrationBuilder.DropTable(
                name: "taxonomies");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "eligibilities");

            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.DropTable(
                name: "organisations");

            migrationBuilder.DropPrimaryKey(
                name: "pk_uicaches",
                table: "uicaches");

            migrationBuilder.DropPrimaryKey(
                name: "pk_servicetypes",
                table: "servicetypes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_relatedorganisations",
                table: "relatedorganisations");

            migrationBuilder.DropPrimaryKey(
                name: "pk_organisationtypes",
                table: "organisationtypes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_adminareas",
                table: "adminareas");

            migrationBuilder.RenameTable(
                name: "uicaches",
                newName: "UICaches");

            migrationBuilder.RenameTable(
                name: "servicetypes",
                newName: "ServiceTypes");

            migrationBuilder.RenameTable(
                name: "relatedorganisations",
                newName: "RelatedOrganisations");

            migrationBuilder.RenameTable(
                name: "organisationtypes",
                newName: "OrganisationTypes");

            migrationBuilder.RenameTable(
                name: "adminareas",
                newName: "AdminAreas");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "UICaches",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "lastmodifiedby",
                table: "UICaches",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "lastmodified",
                table: "UICaches",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "createdby",
                table: "UICaches",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "UICaches",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "UICaches",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "ServiceTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "lastmodifiedby",
                table: "ServiceTypes",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "lastmodified",
                table: "ServiceTypes",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "ServiceTypes",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "createdby",
                table: "ServiceTypes",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "ServiceTypes",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ServiceTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "relatedorganisationid",
                table: "RelatedOrganisations",
                newName: "RelatedOrganisationId");

            migrationBuilder.RenameColumn(
                name: "organisationid",
                table: "RelatedOrganisations",
                newName: "OrganisationId");

            migrationBuilder.RenameColumn(
                name: "lastmodifiedby",
                table: "RelatedOrganisations",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "lastmodified",
                table: "RelatedOrganisations",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "createdby",
                table: "RelatedOrganisations",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "RelatedOrganisations",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "RelatedOrganisations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "OrganisationTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "lastmodifiedby",
                table: "OrganisationTypes",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "lastmodified",
                table: "OrganisationTypes",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "OrganisationTypes",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "createdby",
                table: "OrganisationTypes",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "OrganisationTypes",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "OrganisationTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "lastmodifiedby",
                table: "AdminAreas",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "lastmodified",
                table: "AdminAreas",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "createdby",
                table: "AdminAreas",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "AdminAreas",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "AdminAreas",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AdminAreas",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "organisationid",
                table: "AdminAreas",
                newName: "OpenReferralOrganisationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UICaches",
                table: "UICaches",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceTypes",
                table: "ServiceTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelatedOrganisations",
                table: "RelatedOrganisations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganisationTypes",
                table: "OrganisationTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminAreas",
                table: "AdminAreas",
                column: "Id");

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
