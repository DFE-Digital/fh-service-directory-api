using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fhservicedirectoryapi.api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLinkContactFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_LinkContacts_Locations_LinkId", "LinkContacts");
            migrationBuilder.DropForeignKey("FK_LinkContacts_Organisations_LinkId", "LinkContacts");
            migrationBuilder.DropForeignKey("FK_LinkContacts_ServiceAtLocations_LinkId", "LinkContacts");
            migrationBuilder.DropForeignKey("FK_LinkContacts_Services_LinkId", "LinkContacts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey("FK_LinkContacts_Locations_LinkId", "LinkContacts", "LinkId", "Locations", "Id");
            migrationBuilder.AddForeignKey("FK_LinkContacts_Organisations_LinkId", "LinkContacts", "LinkId", "Organisations", "Id");
            migrationBuilder.AddForeignKey("FK_LinkContacts_ServiceAtLocations_LinkId", "LinkContacts", "LinkId", "ServiceAtLocations", "Id");
            migrationBuilder.AddForeignKey("FK_LinkContacts_Services_LinkId", "LinkContacts", "LinkId", "Services", "Id");
        }
    }
}
