using System.Diagnostics;

namespace fh_service_directory_api.api.Endpoints;

public class MinimalGeneralEndPoints
{
    public void RegisterMinimalGeneralEndPoints(WebApplication app)
    {
        app.MapGet("api/info", (IConfiguration configuration, ILogger < MinimalGeneralEndPoints> logger) =>
        {
            try
            {
                var assembly = typeof(WebMarker).Assembly;

                var creationDate = File.GetCreationTime(assembly.Location);
                var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

                var useDbType = configuration.GetValue<string>("UseDbType");
                if (useDbType != "UseInMemoryDatabase")
                {
                    var connectionString = configuration.GetConnectionString("ServiceDirectoryConnection");
                    var connectionStringOK = false;
                    if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("Database"))
                        connectionStringOK = true;

                    return Results.Ok($"Version: {version}, Last Updated: {creationDate}, Db Type: {useDbType}, Is Connection String OK: {connectionStringOK}");
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
