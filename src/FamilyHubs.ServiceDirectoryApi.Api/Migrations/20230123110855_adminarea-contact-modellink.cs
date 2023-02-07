using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fh_service_directory_api.api.Migrations
{
#pragma warning disable CS8981
    public partial class adminareacontactmodellink : Migration
#pragma warning disable CS8981
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenReferralPhysical_Addresses_OpenReferralLocations_OpenRe~",
                table: "OpenReferralPhysical_Addresses");

            migrationBuilder.DropTable(
                name: "ModelLinks");

            migrationBuilder.DropTable(
                name: "OpenReferralPhones");

            migrationBuilder.DropTable(
                name: "OrganisationAdminDistricts");

            migrationBuilder.AlterColumn<string>(
                name: "OpenReferralLocationId",
                table: "OpenReferralPhysical_Addresses",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telephone",
                table: "OpenReferralContacts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TextPhone",
                table: "OpenReferralContacts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AdminAreas",
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
                    table.PrimaryKey("PK_AdminAreas", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_OpenReferralPhysical_Addresses_OpenReferralLocations_OpenRe~",
                table: "OpenReferralPhysical_Addresses",
                column: "OpenReferralLocationId",
                principalTable: "OpenReferralLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenReferralPhysical_Addresses_OpenReferralLocations_OpenRe~",
                table: "OpenReferralPhysical_Addresses");

            migrationBuilder.DropTable(
                name: "AdminAreas");

            migrationBuilder.DropColumn(
                name: "Telephone",
                table: "OpenReferralContacts");

            migrationBuilder.DropColumn(
                name: "TextPhone",
                table: "OpenReferralContacts");

            migrationBuilder.AlterColumn<string>(
                name: "OpenReferralLocationId",
                table: "OpenReferralPhysical_Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "ModelLinks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LinkType = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ModelOneId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ModelTwoId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenReferralPhones",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Number = table.Column<string>(type: "text", nullable: false),
                    OpenReferralContactId = table.Column<string>(type: "text", nullable: false)
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
                name: "OrganisationAdminDistricts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    OpenReferralOrganisationId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationAdminDistricts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralPhones_OpenReferralContactId",
                table: "OpenReferralPhones",
                column: "OpenReferralContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenReferralPhysical_Addresses_OpenReferralLocations_OpenRe~",
                table: "OpenReferralPhysical_Addresses",
                column: "OpenReferralLocationId",
                principalTable: "OpenReferralLocations",
                principalColumn: "Id");
        }
    }
}
