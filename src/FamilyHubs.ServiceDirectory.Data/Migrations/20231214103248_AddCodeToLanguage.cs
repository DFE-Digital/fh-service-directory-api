using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.ServiceDirectory.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeToLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Languages",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Languages",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            // note: i had to update to the latest version of the dotnet ef tool for this to work
            // dotnet tool update --global dotnet-ef
            migrationBuilder.Sql(@"UPDATE dbo.Languages SET Name = LTRIM(RTRIM(Name))");

            migrationBuilder.Sql(@"UPDATE dbo.Languages
SET Name = CASE
    WHEN Name = 'Persian/Farsi' THEN 'Persian'
    WHEN Name = 'Tagalog/Filipino' THEN 'Tagalog'
    ELSE Name
    END
");

            migrationBuilder.Sql(@"UPDATE dbo.Languages
SET Code = CASE
    WHEN Name = 'Arabic' THEN 'ar'
    WHEN Name = 'Bengali' THEN 'bn'
    WHEN Name = 'Chinese' THEN 'zh'
    WHEN Name = 'English' THEN 'en'
    WHEN Name = 'French' THEN 'fr'
    WHEN Name = 'German' THEN 'de'
    WHEN Name = 'Gujarati' THEN 'gu'
    WHEN Name = 'Italian' THEN 'it'
    WHEN Name = 'Lithuanian' THEN 'lt'
    WHEN Name = 'Persian' THEN 'fa'
    WHEN Name = 'Polish' THEN 'pl'
    WHEN Name = 'Portuguese' THEN 'pt'
    WHEN Name = 'Punjabi' THEN 'pa'
    WHEN Name = 'Romanian' THEN 'ro'
    WHEN Name = 'Russian' THEN 'ru'
    WHEN Name = 'Somali' THEN 'so'
    WHEN Name = 'Spanish' THEN 'es'
    WHEN Name = 'Tagalog' THEN 'tl'
    WHEN Name = 'Tamil' THEN 'ta'
    WHEN Name = 'Turkish' THEN 'tr'
    WHEN Name = 'Ukrainian' THEN 'uk'
    WHEN Name = 'Urdu' THEN 'ur'
    END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Languages");
        }
    }
}
