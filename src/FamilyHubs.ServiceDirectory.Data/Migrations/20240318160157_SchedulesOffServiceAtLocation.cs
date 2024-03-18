using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.ServiceDirectory.Data.Migrations
{
    /// <inheritdoc />
    public partial class SchedulesOffServiceAtLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceAtLocations",
                table: "ServiceAtLocations");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "ServiceAtLocations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ServiceAtLocations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ServiceAtLocations",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ServiceAtLocations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "ServiceAtLocations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "ServiceAtLocations",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ServiceAtLocationId",
                table: "Schedules",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceAtLocations",
                table: "ServiceAtLocations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAtLocations_LocationId",
                table: "ServiceAtLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ServiceAtLocationId",
                table: "Schedules",
                column: "ServiceAtLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_ServiceAtLocations_ServiceAtLocationId",
                table: "Schedules",
                column: "ServiceAtLocationId",
                principalTable: "ServiceAtLocations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_ServiceAtLocations_ServiceAtLocationId",
                table: "Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceAtLocations",
                table: "ServiceAtLocations");

            migrationBuilder.DropIndex(
                name: "IX_ServiceAtLocations_LocationId",
                table: "ServiceAtLocations");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_ServiceAtLocationId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ServiceAtLocations");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ServiceAtLocations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ServiceAtLocations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ServiceAtLocations");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "ServiceAtLocations");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "ServiceAtLocations");

            migrationBuilder.DropColumn(
                name: "ServiceAtLocationId",
                table: "Schedules");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceAtLocations",
                table: "ServiceAtLocations",
                columns: new[] { "LocationId", "ServiceId" });
        }
    }
}
