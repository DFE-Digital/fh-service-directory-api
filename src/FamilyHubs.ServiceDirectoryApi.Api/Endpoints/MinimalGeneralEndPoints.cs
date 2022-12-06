using fh_service_directory_api.infrastructure.Persistence.Repository;
using Microsoft.AspNetCore.Mvc;
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

                string useDbType = configuration.GetValue<string>("UseDbType");
                if (useDbType != "UseInMemoryDatabase")
                {
                    string? connectionString = configuration.GetConnectionString("ServiceDirectoryConnection");
                    bool connectionStringOK = false;
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

        app.MapGet("api/resetdb", async (string password, IConfiguration configuration, ApplicationDbContextInitialiser initialiser, ILogger <MinimalGeneralEndPoints> logger) =>
        {
            try
            {
                if (password== "RnFkxAFcJLF9kcA4BYv4")
                {
                    string useDbType = configuration.GetValue<string>("UseDbType");
                    if (useDbType != "UseSqlServerDatabase" || useDbType != "UsePostgresDatabase")
                    {
                        await initialiser.InitialiseWithRecreateAsync(configuration);
                        await initialiser.SeedAsync();
                    }
                }

                return Results.Ok("All Done");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting info (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).ExcludeFromDescription();
    }
}
