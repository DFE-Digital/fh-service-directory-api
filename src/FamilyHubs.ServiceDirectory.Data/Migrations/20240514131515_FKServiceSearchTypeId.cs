using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.ServiceDirectory.Data.Migrations
{
    /// <inheritdoc />
    public partial class FKServiceSearchTypeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "ServiceSearchTypeId",
                table: "ServiceSearches",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSearches_ServiceSearchTypeId",
                table: "ServiceSearches",
                column: "ServiceSearchTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSearches_ServiceTypes_ServiceSearchTypeId",
                table: "ServiceSearches",
                column: "ServiceSearchTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSearches_ServiceTypes_ServiceSearchTypeId",
                table: "ServiceSearches");

            migrationBuilder.DropIndex(
                name: "IX_ServiceSearches_ServiceSearchTypeId",
                table: "ServiceSearches");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceSearchTypeId",
                table: "ServiceSearches",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }
    }
}
