#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace fh_service_directory_api.api.Migrations
{
    public partial class LinkTaxonomy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpenReferralLinktaxonomycollections");

            migrationBuilder.CreateTable(
                name: "OpenReferralLinkTaxonomies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TaxonomyId = table.Column<string>(type: "text", nullable: true),
                    LinkId = table.Column<string>(type: "text", nullable: false),
                    LinkType = table.Column<string>(type: "text", nullable: false),
                    OpenReferralLocationId = table.Column<string>(type: "text", nullable: true),
                    OpenReferralParentId = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpenReferralLinkTaxonomies");

            migrationBuilder.CreateTable(
                name: "OpenReferralLinktaxonomycollections",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Link_id = table.Column<string>(type: "text", nullable: false),
                    Link_type = table.Column<string>(type: "text", nullable: false),
                    OpenReferralParentId = table.Column<string>(type: "text", nullable: true),
                    OpenReferralServiceId = table.Column<string>(type: "text", nullable: false),
                    OpenReferralTaxonomyId = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralLinktaxonomycollections_OpenReferralParentId",
                table: "OpenReferralLinktaxonomycollections",
                column: "OpenReferralParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenReferralLinktaxonomycollections_OpenReferralTaxonomyId",
                table: "OpenReferralLinktaxonomycollections",
                column: "OpenReferralTaxonomyId");
        }
    }
}
