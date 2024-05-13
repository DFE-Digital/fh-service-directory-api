using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FamilyHubs.ServiceDirectory.Data.Migrations
{
    /// <inheritdoc />
    public partial class MetricEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceSearches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SearchTriggerEventId = table.Column<short>(type: "smallint", nullable: false),
                    ServiceSearchType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchPostcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchRadiusMiles = table.Column<byte>(type: "tinyint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    HttpResponseCode = table.Column<byte>(type: "tinyint", nullable: true),
                    RequestTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponseTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CorrelationId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceSearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceSearches_Events_SearchTriggerEventId",
                        column: x => x.SearchTriggerEventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceSearchResults",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceSearchId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceSearchResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceSearchResults_ServiceSearches_ServiceSearchId",
                        column: x => x.ServiceSearchId,
                        principalTable: "ServiceSearches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceSearchResults_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { (short)1, "Describes an initial, unfiltered search by a user.", "ServiceDirectoryInitialSearch" },
                    { (short)2, "Describes a filtered search by a user.", "ServiceDirectorySearchFilter" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSearches_SearchTriggerEventId",
                table: "ServiceSearches",
                column: "SearchTriggerEventId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSearchResults_ServiceId",
                table: "ServiceSearchResults",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSearchResults_ServiceSearchId",
                table: "ServiceSearchResults",
                column: "ServiceSearchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceSearchResults");

            migrationBuilder.DropTable(
                name: "ServiceSearches");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
