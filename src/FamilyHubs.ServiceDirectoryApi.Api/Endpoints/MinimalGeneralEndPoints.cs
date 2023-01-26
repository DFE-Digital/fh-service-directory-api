using System.Diagnostics;

namespace FamilyHubs.ServiceDirectory.Api.Endpoints;

public class MinimalGeneralEndPoints
{
    public void RegisterMinimalGeneralEndPoints(WebApplication app)
    {
        app.MapGet("api/info", (IConfiguration configuration, ILogger < MinimalGeneralEndPoints> logger) =>
        {
            try
            {
                var assembly = typeof(MinimalGeneralEndPoints).Assembly;

                var creationDate = File.GetCreationTime(assembly.Location);
                var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

                var useDbType = configuration.GetValue<string>("UseDbType");
                if (useDbType != "UseInMemoryDatabase")
                {
                    var connectionString = configuration.GetConnectionString("ServiceDirectoryConnection");
                    var connectionStringOk = !string.IsNullOrEmpty(connectionString) && connectionString.Contains("Database");

                    return Results.Ok($"Version: {version}, Last Updated: {creationDate}, Db Type: {useDbType}, Is Connection String OK: {connectionStringOk}");
                }
                

                return Results.Ok($"Version: {version}, Last Updated: {creationDate}, Db Type: {useDbType}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting info (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        });
    }
}
