using fh_service_directory_api.api.Queries.ListOrganisation;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
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

        app.MapGet("api/test", (ILoggerFactory loggerFactory, ILogger<MinimalGeneralEndPoints> logger, IConfiguration configuration) =>
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

                return Results.Ok($"Version: {version}, Last Updated: {creationDate}, Environment: {env} RecreateDbOnStartup: {configuration.GetValue<bool>("RecreateDbOnStartup")} UseInMemoryDatabase: {configuration.GetValue<bool>("UseInMemoryDatabase")} UseSqlServerDatabase: {configuration.GetValue<bool>("UseSqlServerDatabase")}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting info (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        });

        app.MapGet("api/testorganizations", async (CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalOrganisationEndPoints> logger, ApplicationDbContext context) =>
        {
            try
            {
                ListOpenReferralOrganisationCommand request = new();
                return Results.Ok($"Created ListOpenReferralOrganisationCommand");
                //var result = await _mediator.Send(request, cancellationToken);
                //return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred listing organisation (api). {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        });
    }
}
