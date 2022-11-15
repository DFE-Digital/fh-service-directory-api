using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fh_service_directory_api.api.Migrations
{
    public partial class CreateIntialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModelLinks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    LinkType = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ModelOneId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ModelTwoId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralLocations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_OpenReferralLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralParents",
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
                    table.PrimaryKey("PK_OpenReferralParents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganisationAdminDistricts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OpenReferralOrganisationId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationAdminDistricts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganisationTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelatedOrganisations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    OrganisationId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RelatedOrganisationId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedOrganisations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UICaches",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UICaches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accessibility_For_Disabilities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Accessibility = table.Column<string>(type: "text", nullable: false),
                    OpenReferralLocationId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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
                    Address_1 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Postal_code = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Country = table.Column<string>(type: "text", nullable: true),
                    State_province = table.Column<string>(type: "text", nullable: true),
                    OpenReferralLocationId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralPhysical_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralPhysical_Addresses_OpenReferralLocations_OpenRe~",
                        column: x => x.OpenReferralLocationId,
                        principalTable: "OpenReferralLocations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralOrganisations",
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
                    table.PrimaryKey("PK_OpenReferralOrganisations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralOrganisations_OrganisationTypes_OrganisationTyp~",
                        column: x => x.OrganisationTypeId,
                        principalTable: "OrganisationTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralServices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ServiceTypeId = table.Column<string>(type: "text", nullable: true),
                    OpenReferralOrganisationId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Accreditations = table.Column<string>(type: "text", nullable: true),
                    Assured_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Attending_access = table.Column<string>(type: "text", nullable: true),
                    Attending_type = table.Column<string>(type: "text", nullable: true),
                    Deliverable_type = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Fees = table.Column<string>(type: "text", nullable: true),
                    CanFamilyChooseDeliveryLocation = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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
                    Amount_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    LinkId = table.Column<string>(type: "text", nullable: true),
                    Option = table.Column<string>(type: "text", nullable: true),
                    Valid_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Valid_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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
                    Eligibility = table.Column<string>(type: "text", nullable: false),
                    LinkId = table.Column<string>(type: "text", nullable: true),
                    Maximum_age = table.Column<int>(type: "integer", nullable: false),
                    Minimum_age = table.Column<int>(type: "integer", nullable: false),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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
                    Source = table.Column<string>(type: "text", nullable: false),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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
                    Language = table.Column<string>(type: "text", nullable: false),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Score = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true),
                    Widget = table.Column<string>(type: "text", nullable: true),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false),
                    OpenReferralOrganisationId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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
                    Service_area = table.Column<string>(type: "text", nullable: false),
                    LinkId = table.Column<string>(type: "text", nullable: true),
                    Extent = table.Column<string>(type: "text", nullable: true),
                    Uri = table.Column<string>(type: "text", nullable: true),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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
                    ServiceDelivery = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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
                name: "OpenReferralPhones",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: false),
                    OpenReferralContactId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralPhones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralPhones_OpenReferralContacts_OpenReferralContact~",
                        column: x => x.OpenReferralContactId,
                        principalTable: "OpenReferralContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralTaxonomies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Vocabulary = table.Column<string>(type: "text", nullable: true),
                    Parent = table.Column<string>(type: "text", nullable: true),
                    OpenReferralEligibilityId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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
                    Closes_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    End_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Opens_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OpenReferralServiceAtLocationId = table.Column<string>(type: "text", nullable: false),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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
                    Description = table.Column<string>(type: "text", nullable: false),
                    Opens_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Closes_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Byday = table.Column<string>(type: "text", nullable: true),
                    Bymonthday = table.Column<string>(type: "text", nullable: true),
                    Dtstart = table.Column<string>(type: "text", nullable: true),
                    Freq = table.Column<string>(type: "text", nullable: true),
                    Interval = table.Column<string>(type: "text", nullable: true),
                    Valid_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Valid_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OpenReferralServiceAtLocationId = table.Column<string>(type: "text", nullable: false),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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
                name: "OpenReferralLinktaxonomycollections",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Link_id = table.Column<string>(type: "text", nullable: false),
                    Link_type = table.Column<string>(type: "text", nullable: false),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false),
                    OpenReferralParentId = table.Column<string>(type: "text", nullable: true),
                    OpenReferralTaxonomyId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenReferralLinktaxonomycollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenReferralLinktaxonomycollections_OpenReferralParents_Ope~",
                        column: x => x.OpenReferralParentId,
                        principalTable: "OpenReferralParents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OpenReferralLinktaxonomycollections_OpenReferralTaxonomies_~",
                        column: x => x.OpenReferralTaxonomyId,
                        principalTable: "OpenReferralTaxonomies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralService_Taxonomies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    LinkId = table.Column<string>(type: "text", nullable: true),
                    TaxonomyId = table.Column<string>(type: "text", nullable: true),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false),
                    OpenReferralParentId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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
                name: "IX_OpenReferralLinktaxonomycollections_OpenReferralParentId",
                table: "OpenReferralLinktaxonomycollections",
                column: "OpenReferralParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralLinktaxonomycollections_OpenReferralTaxonomyId",
                table: "OpenReferralLinktaxonomycollections",
                column: "OpenReferralTaxonomyId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralOrganisations_OrganisationTypeId",
                table: "OpenReferralOrganisations",
                column: "OrganisationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralPhones_OpenReferralContactId",
                table: "OpenReferralPhones",
                column: "OpenReferralContactId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accessibility_For_Disabilities");

            migrationBuilder.DropTable(
                name: "ModelLinks");

            migrationBuilder.DropTable(
                name: "OpenReferralCost_Options");

            migrationBuilder.DropTable(
                name: "OpenReferralFundings");

            migrationBuilder.DropTable(
                name: "OpenReferralHoliday_Schedules");

            migrationBuilder.DropTable(
                name: "OpenReferralLanguages");

            migrationBuilder.DropTable(
                name: "OpenReferralLinktaxonomycollections");

            migrationBuilder.DropTable(
                name: "OpenReferralPhones");

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
                name: "OrganisationAdminDistricts");

            migrationBuilder.DropTable(
                name: "RelatedOrganisations");

            migrationBuilder.DropTable(
                name: "UICaches");

            migrationBuilder.DropTable(
                name: "OpenReferralContacts");

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

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "OrganisationTypes");
        }
    }
}
