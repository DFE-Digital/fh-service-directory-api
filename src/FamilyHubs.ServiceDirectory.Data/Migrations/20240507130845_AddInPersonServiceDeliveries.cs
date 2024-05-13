using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.ServiceDirectory.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInPersonServiceDeliveries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
@"INSERT INTO ServiceDeliveries (ServiceId, Name, Created, LastModified, CreatedBy, LastModifiedBy)
SELECT Id, 'InPerson', GETDATE(), GETDATE(), 'System', 'System'
FROM Services
WHERE Id IN (
  SELECT ServiceId
  FROM ServiceAtLocations
  WHERE ServiceId IN (
    SELECT ServiceId
    FROM ServiceDeliveries
    WHERE ServiceId NOT IN (
      SELECT ServiceId
      FROM ServiceDeliveries
      WHERE Name = 'InPerson'
    )
  )
)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
