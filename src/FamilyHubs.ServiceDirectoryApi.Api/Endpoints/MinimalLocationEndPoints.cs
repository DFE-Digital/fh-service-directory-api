using System.Diagnostics;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateLocation;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateLocation;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FamilyHubs.ServiceDirectory.Api.Endpoints;

public class MinimalLocationEndPoints
{
    public void RegisterLocationEndPoints(WebApplication app)
    {
        app.MapPost("api/location/{taxonomyId}/{organisationId}", [Authorize(Policy = "ServiceAccess")] async (string taxonomyId, string organisationId,[FromBody] LocationDto request, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalTaxonomyEndPoints> logger) =>
        {
            try
            {
                CreateLocationCommand command = new(request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating location. {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Location", "Create Location") { Tags = new[] { "Locations" } });

        app.MapPut("api/location/{taxonomyId}/{organisationId}", async (string taxonomyId, string organisationId, [FromBody] LocationDto request, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalTaxonomyEndPoints> logger) =>
        {
            try
            {
                UpdateLocationCommand command = new(request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred updating location. {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Location", "Update Location As Family Hub") { Tags = new[] { "Locations" } });
    }
}
