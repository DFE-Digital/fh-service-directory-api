using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.ServiceDirectory.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToServiceAndContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contacts_ServiceId",
                table: "Contacts");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceType_OrganisationId_Status",
                table: "Services",
                columns: new[] { "ServiceType", "Id", "OrganisationId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ServiceId_Id",
                table: "Contacts",
                column: "ServiceId")
                .Annotation("SqlServer:Include", new[] { "Id", "Title", "Name", "Telephone", "TextPhone", "Url", "Email" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServiceType_OrganisationId_Status",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_ServiceId_Id",
                table: "Contacts");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ServiceId",
                table: "Contacts",
                column: "ServiceId");
        }
    }
}
