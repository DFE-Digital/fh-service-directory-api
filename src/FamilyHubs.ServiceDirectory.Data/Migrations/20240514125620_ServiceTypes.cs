using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FamilyHubs.ServiceDirectory.Data.Migrations
{
    /// <inheritdoc />
    public partial class ServiceTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceSearchType",
                table: "ServiceSearches");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "ServiceSearches",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "ServiceSearchTypeId",
                table: "ServiceSearches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    nvarchar255 = table.Column<string>(name: "nvarchar(255)", type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "Id", "nvarchar(255)", "Name" },
                values: new object[,]
                {
                    { (byte)1, "Connect", "InformationSharing" },
                    { (byte)2, "Find", "FamilyExperience" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropColumn(
                name: "ServiceSearchTypeId",
                table: "ServiceSearches");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "ServiceSearches",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceSearchType",
                table: "ServiceSearches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
