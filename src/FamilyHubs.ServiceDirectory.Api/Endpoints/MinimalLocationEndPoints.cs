using FamilyHubs.ServiceDirectory.Core.Commands.Locations.CreateLocation;
using FamilyHubs.ServiceDirectory.Core.Commands.Locations.UpdateLocation;
using FamilyHubs.ServiceDirectory.Core.Queries.Locations.GetLocationById;
using FamilyHubs.ServiceDirectory.Core.Queries.Locations.GetLocationsByOrganisationId;
using FamilyHubs.ServiceDirectory.Core.Queries.Locations.GetLocationsByServiceId;
using FamilyHubs.ServiceDirectory.Core.Queries.Locations.ListLocations;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.SharedKernel.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FamilyHubs.ServiceDirectory.Api.Endpoints;

public class MinimalLocationEndPoints
{
    public void RegisterLocationEndPoints(WebApplication app)
    {
        app.MapGet("api/locations", async (int? pageNumber, string? orderByColumn, int? pageSize, bool? isAscending, string? searchName, bool? isFamilyHub, bool? isNonFamilyHub, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalLocationEndPoints> logger) =>
        {
            try
            {
                var command = new ListLocationsCommand(pageNumber, orderByColumn, pageSize, isAscending, searchName, isFamilyHub, isNonFamilyHub);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred listing open referral locations. {exceptionMessage}", ex.Message);

                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("List Locations", "List Locations") { Tags = new[] { "Locations" } });

        app.MapGet("api/locations/{id}", async (long id, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalLocationEndPoints> logger) =>
        {
            try
            {
                var command = new GetLocationByIdCommand { Id = id };
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting open referral location by id. {exceptionMessage}", ex.Message);

                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Location by Id", "Get Location by Id") { Tags = new[] { "Locations" } });

        app.MapGet("api/organisationlocations/{id}", async (long id, int? pageNumber, string? orderByColumn, int? pageSize, bool? isAscending, string? searchName, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalLocationEndPoints> logger) =>
        {
            try
            {
                var command = new GetLocationsByOrganisationIdCommand(id, pageNumber, pageSize, isAscending, orderByColumn, searchName);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting open referral location by id. {exceptionMessage}",
                    ex.Message);

                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Locations by Organisation Id", "Get Location by Organisation Id") { Tags = new[] { "Locations" } });

        app.MapGet("api/servicelocations/{id}", async (long id, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalLocationEndPoints> logger) =>
        {
            try
            {
                var command = new GetLocationsByServiceIdCommand { ServiceId = id };
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting open referral location by id. {exceptionMessage}",
                    ex.Message);

                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Locations by service Id", "Get Location by Service Id") { Tags = new[] { "Locations" } });

        app.MapPut("api/locations/{id}",
            [Authorize(Roles = $"{RoleTypes.DfeAdmin},{RoleTypes.LaManager},{RoleTypes.LaDualRole}")] async 
            (long id, 
            [FromBody] LocationDto request, 
            CancellationToken cancellationToken, 
            ISender mediator, 
            ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var command = new UpdateLocationCommand(id, request);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred updating location (api). {exceptionMessage}", ex.Message);

                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update a Location", "Update a Location") { Tags = new[] { "Locations" } });

        app.MapPost("api/locations",
            [Authorize(Roles = $"{RoleTypes.DfeAdmin},{RoleTypes.LaManager},{RoleTypes.LaDualRole}")] async 
            ([FromBody] LocationDto request, 
            CancellationToken cancellationToken, 
            ISender mediator, 
            ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var command = new CreateLocationCommand(request);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating location (api). {exceptionMessage}", ex.Message);

                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Create a Location", "Create a Location") { Tags = new[] { "Locations" } });
    }
}