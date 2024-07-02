using System.Diagnostics;

namespace FamilyHubs.ServiceDirectory.Api.Endpoints;

public class MinimalGeneralEndPoints
{
    public void RegisterGeneralEndPoints(WebApplication app)
    {
        app.MapGet("api/info", (IConfiguration configuration, ILogger < MinimalGeneralEndPoints> logger) =>
        {
            try
            {
                var assembly = typeof(MinimalGeneralEndPoints).Assembly;

                var creationDate = File.GetCreationTime(assembly.Location);
                var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

                var useSqlite = configuration.GetValue<bool>("UseSqlite");

                var connectionString = configuration.GetConnectionString("ServiceDirectoryConnection");
                var connectionStringOk = !string.IsNullOrEmpty(connectionString) && connectionString.Contains("Database");

                return Results.Ok($"Version: {version}, Last Updated: {creationDate}, Db Type: {(useSqlite ? "Sqlite" : "SQLServer")}, Is Connection String OK: {connectionStringOk}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting info (api). {ExceptionMessage}", ex.Message);
                
                throw;
            }
        });
    }
}
