using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.ServiceDirectory.Data.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleAddOpenReferralFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "Weekday",
               table: "Schedules");

            migrationBuilder.AddColumn<int>(
                table: "Schedules",
                name: "Timezone",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Interval",
                table: "Schedules",
                type: "int",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttendingType",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ByWeekNo",
                table: "Schedules",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ByYearDay",
                table: "Schedules",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Schedules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScheduleLink",
                table: "Schedules",
                type: "nvarchar(600)",
                maxLength: 600,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Until",
                table: "Schedules",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WkSt",
                table: "Schedules",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendingType",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ByWeekNo",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ByYearDay",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ScheduleLink",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Until",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "WkSt",
                table: "Schedules");

            migrationBuilder.DropColumn(
               name: "Timezone",
               table: "Schedules");

            migrationBuilder.AddColumn<int>(
                table: "Schedules",
                name: "Interval",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }
    }
}
