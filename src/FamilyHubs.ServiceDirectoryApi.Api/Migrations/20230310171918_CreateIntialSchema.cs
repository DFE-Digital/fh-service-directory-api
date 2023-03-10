using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FamilyHubs.ServiceDirectory.Api.Migrations
{
    /// <inheritdoc />
    public partial class CreateIntialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contacts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    telephone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    textphone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    locationtype = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    address1 = table.Column<string>(type: "text", nullable: false),
                    address2 = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    postcode = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    stateprovince = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false),
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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    organisationtype = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    adminareacode = table.Column<string>(type: "text", nullable: false),
                    associatedorganisationid = table.Column<long>(type: "bigint", nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "taxonomies",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    taxonomytype = table.Column<string>(type: "text", nullable: false),
                    parentid = table.Column<long>(type: "bigint", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_taxonomies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "accessibilityfordisabilities",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    accessibility = table.Column<string>(type: "text", nullable: true),
                    locationid = table.Column<long>(type: "bigint", nullable: false),
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
                name: "services",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    serviceownerreferenceid = table.Column<string>(type: "text", nullable: false),
                    servicetype = table.Column<string>(type: "text", nullable: false),
                    organisationid = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    fees = table.Column<string>(type: "text", nullable: true),
                    accreditations = table.Column<string>(type: "text", nullable: true),
                    deliverabletype = table.Column<string>(type: "text", nullable: false),
                    assureddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    attendingtype = table.Column<string>(type: "text", nullable: false),
                    attendingaccess = table.Column<string>(type: "text", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "costoptions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    validfrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    validto = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    option = table.Column<string>(type: "text", nullable: true),
                    amount = table.Column<decimal>(type: "numeric", nullable: true),
                    amountdescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    serviceid = table.Column<long>(type: "bigint", nullable: false),
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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    eligibilitytype = table.Column<string>(type: "text", nullable: false),
                    maximumage = table.Column<int>(type: "integer", nullable: false),
                    minimumage = table.Column<int>(type: "integer", nullable: false),
                    serviceid = table.Column<long>(type: "bigint", nullable: false),
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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    source = table.Column<string>(type: "text", nullable: true),
                    serviceid = table.Column<long>(type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
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
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "holidayschedules",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    closed = table.Column<bool>(type: "boolean", nullable: false),
                    opensat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    closesat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    startdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    enddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    serviceid = table.Column<long>(type: "bigint", nullable: true),
                    locationid = table.Column<long>(type: "bigint", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_holidayschedules", x => x.id);
                    table.ForeignKey(
                        name: "fk_holidayschedules_locations_locationid",
                        column: x => x.locationid,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_holidayschedules_services_serviceid",
                        column: x => x.serviceid,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "languages",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    serviceid = table.Column<long>(type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
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
                name: "linkcontacts",
                columns: table => new
                {
                    contactid = table.Column<long>(type: "bigint", nullable: false),
                    linkid = table.Column<long>(type: "bigint", nullable: false),
                    linktype = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_linkcontacts", x => new { x.contactid, x.linkid });
                    table.ForeignKey(
                        name: "fk_linkcontacts_contacts_contactid",
                        column: x => x.contactid,
                        principalTable: "contacts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "regularschedules",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    weekday = table.Column<string>(type: "text", nullable: false),
                    opensat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    closesat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    validfrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    validto = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    dtstart = table.Column<string>(type: "text", nullable: true),
                    freq = table.Column<string>(type: "text", nullable: false),
                    interval = table.Column<string>(type: "text", nullable: true),
                    byday = table.Column<string>(type: "text", nullable: true),
                    bymonthday = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    serviceid = table.Column<long>(type: "bigint", nullable: true),
                    locationid = table.Column<long>(type: "bigint", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lastmodified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_regularschedules", x => x.id);
                    table.ForeignKey(
                        name: "fk_regularschedules_locations_locationid",
                        column: x => x.locationid,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_regularschedules_services_serviceid",
                        column: x => x.serviceid,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    score = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    widget = table.Column<string>(type: "text", nullable: true),
                    serviceid = table.Column<long>(type: "bigint", nullable: true),
                    organisationid = table.Column<long>(type: "bigint", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
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
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    serviceareaname = table.Column<string>(type: "text", nullable: true),
                    extent = table.Column<string>(type: "text", nullable: true),
                    uri = table.Column<string>(type: "text", nullable: true),
                    serviceid = table.Column<long>(type: "bigint", nullable: false),
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
                    serviceid = table.Column<long>(type: "bigint", nullable: false),
                    locationid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_serviceatlocations", x => new { x.locationid, x.serviceid });
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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    serviceid = table.Column<long>(type: "bigint", nullable: false),
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
                name: "servicetaxonomies",
                columns: table => new
                {
                    serviceid = table.Column<long>(type: "bigint", nullable: false),
                    taxonomyid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_servicetaxonomies", x => new { x.serviceid, x.taxonomyid });
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
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "linktaxonomies",
                columns: table => new
                {
                    taxonomyid = table.Column<long>(type: "bigint", nullable: false),
                    linkid = table.Column<long>(type: "bigint", nullable: false),
                    linktype = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_linktaxonomies", x => new { x.linkid, x.taxonomyid });
                    table.ForeignKey(
                        name: "fk_linktaxonomies_taxonomies_taxonomyid",
                        column: x => x.taxonomyid,
                        principalTable: "taxonomies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "ix_holidayschedules_locationid",
                table: "holidayschedules",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "ix_holidayschedules_serviceid",
                table: "holidayschedules",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "ix_languages_serviceid",
                table: "languages",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "ix_linkcontacts_linkid",
                table: "linkcontacts",
                column: "linkid");

            migrationBuilder.CreateIndex(
                name: "ix_linktaxonomies_taxonomyid",
                table: "linktaxonomies",
                column: "taxonomyid");

            migrationBuilder.CreateIndex(
                name: "ix_regularschedules_locationid",
                table: "regularschedules",
                column: "locationid");

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
                name: "ix_servicetaxonomies_taxonomyid",
                table: "servicetaxonomies",
                column: "taxonomyid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accessibilityfordisabilities");

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
                name: "regularschedules");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "serviceatlocations");

            migrationBuilder.DropTable(
                name: "servicedeliveries");

            migrationBuilder.DropTable(
                name: "servicetaxonomies");

            migrationBuilder.DropTable(
                name: "contacts");

            migrationBuilder.DropTable(
                name: "costoptions");

            migrationBuilder.DropTable(
                name: "eligibilities");

            migrationBuilder.DropTable(
                name: "serviceareas");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "taxonomies");

            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.DropTable(
                name: "organisations");
        }
    }
}
