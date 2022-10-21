using System.Diagnostics;

namespace fh_service_directory_api.api.Endpoints;

public class MinimalGeneralEndPoints
{
    public void RegisterMinimalGeneralEndPoints(WebApplication app)
    {
        app.MapGet("api/info", (ILogger<MinimalGeneralEndPoints> logger) =>
        {
            try
            {
                var assembly = typeof(WebMarker).Assembly;

                var creationDate = File.GetCreationTime(assembly.Location);
                var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

                return Results.Ok($"Version: {version}, Last Updated: {creationDate}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting info (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        });

        app.MapGet("api/test", (ILoggerFactory loggerFactory, ILogger<MinimalGeneralEndPoints> logger) =>
        {
            try
            {
                var assembly = typeof(WebMarker).Assembly;

                var creationDate = File.GetCreationTime(assembly.Location);
                var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                logger.LogInformation($"api/Test - Version: {version}, Last Updated: {creationDate}, Environment: {env}");

                var logger1 = loggerFactory.CreateLogger<MinimalGeneralEndPoints>();
                logger1.LogWarning("This is a WARNING message");
                logger1.LogInformation("This is an INFORMATION message");

                return Results.Ok($"Version: {version}, Last Updated: {creationDate}, Environment: {env}");
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
