using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace FamilyHubs.ServiceDirectory.Data.Migrations
{
    /// <inheritdoc />
    public partial class SpatialColumnOnLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "GeoPoint",
                table: "Locations",
                type: "geography",
                nullable: true
            );

            migrationBuilder.Sql("UPDATE [Locations] SET [GeoPoint] = geography::Point([Latitude], [Longitude], 4326);");
            
            migrationBuilder.AlterColumn<Point>(
                name: "GeoPoint",
                table: "Locations",
                type: "geography",
                nullable: false
            );

            migrationBuilder.Sql("CREATE SPATIAL INDEX [IX_Locations_Location] ON [Locations] ( [GeoPoint] ) USING GEOGRAPHY_GRID;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex("IX_Locations_Location", "Locations");
            migrationBuilder.DropColumn("GeoPoint", "Locations");
        }
    }
}
