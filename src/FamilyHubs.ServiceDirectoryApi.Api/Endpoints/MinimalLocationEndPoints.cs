using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using fh_service_directory_api.api.Commands.CreateLocation;
using fh_service_directory_api.api.Commands.UpdateLocation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace fh_service_directory_api.api.Endpoints;

public class MinimalLocationEndPoints
{
    public void RegisterLocationEndPoints(WebApplication app)
    {
        app.MapPost("api/location/{taxonomyId}/{organisationId}", [Authorize(Policy = "ServiceAccess")] async (string taxonomyId, string organisationId,[FromBody] OpenReferralLocationDto request, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalTaxonomyEndPoints> logger) =>
        {
            try
            {
                CreateOpenReferralLocationCommand command = new(request, taxonomyId, organisationId);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating location. {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Location", "Create Location") { Tags = new[] { "Locations" } });

        app.MapPut("api/location/{taxonomyId}/{organisationId}", async (string taxonomyId, string organisationId, [FromBody] OpenReferralLocationDto request, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalTaxonomyEndPoints> logger) =>
        {
            try
            {
                UpdateOpenReferralLocationCommand command = new(request, taxonomyId, organisationId);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred updating location. {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Location", "Update Location As Family Hub") { Tags = new[] { "Locations" } });
    }
}
