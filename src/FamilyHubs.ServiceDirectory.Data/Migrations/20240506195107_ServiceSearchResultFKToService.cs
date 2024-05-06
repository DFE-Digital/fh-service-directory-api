using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.ServiceDirectory.Data.Migrations
{
    /// <inheritdoc />
    public partial class ServiceSearchResultFKToService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ServiceSearchResults_ServiceId",
                table: "ServiceSearchResults",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSearchResults_Services_ServiceId",
                table: "ServiceSearchResults",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSearchResults_Services_ServiceId",
                table: "ServiceSearchResults");

            migrationBuilder.DropIndex(
                name: "IX_ServiceSearchResults_ServiceId",
                table: "ServiceSearchResults");
        }
    }
}
