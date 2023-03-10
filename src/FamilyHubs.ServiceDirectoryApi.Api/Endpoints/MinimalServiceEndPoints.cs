using System.Diagnostics;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.DeleteService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Api.Queries.GetService;
using FamilyHubs.ServiceDirectory.Api.Queries.GetServices;
using FamilyHubs.ServiceDirectory.Api.Queries.GetServicesByOrganisation;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FamilyHubs.ServiceDirectory.Api.Endpoints;

public class MinimalServiceEndPoints
{
    public void RegisterServiceEndPoints(WebApplication app)
    {
        app.MapGet("api/services", async (ServiceType serviceType, ServiceStatusType status, string ? districtCode, int ? minimumAge, int? maximumAge, int? givenAge, double? latitude, double? longitude, double? proximity, int? pageNumber, int? pageSize, string? text, string? serviceDeliveries, bool? isPaidFor, string? taxonomyIds, string ? languages, bool? canFamilyChooseLocation, bool? isFamilyHub, int? maxFamilyHubs, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalServiceEndPoints> logger) =>
        {
            try
            {
                var command = new GetServicesCommand(serviceType, status, districtCode, minimumAge,
                    maximumAge, givenAge, latitude, longitude, proximity, pageNumber, pageSize, text,
                    serviceDeliveries, isPaidFor, taxonomyIds, languages, canFamilyChooseLocation, isFamilyHub,
                    maxFamilyHubs);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred listing open referral services. {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("List Services", "List Services") { Tags = new[] { "Services" } });

        app.MapGet("api/services/{id}", async (long id, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalServiceEndPoints> logger) =>
        {
            try
            {
                var command = new GetServiceByIdCommand(id);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting open referral service by id. {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Service by Id", "Get Service by Id") { Tags = new[] { "Services" } });

        app.MapDelete("api/services/{id}", async (long id, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalServiceEndPoints> logger) =>
        {
            try
            {
                var command = new DeleteServiceByIdCommand(id);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred deleting open referral service by id. {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Delete Service by Id", "Delete Service by Id") { Tags = new[] { "Services" } });

        app.MapGet("api/organisationservices/{id}", async (long id, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalServiceEndPoints> logger) =>
        {
            try
            {
                var command = new GetServicesByOrganisationIdCommand(id);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting open referral service by id. {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Services by Organisation Id", "Get Service by Organisation Id") { Tags = new[] { "Services" } });

        app.MapPost("api/services", [Authorize(Policy = "ServiceAccess")] async ([FromBody] ServiceDto request, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var command = new CreateServiceCommand(request);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating service (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Create a Service", "Create a Service") { Tags = new[] { "Services" } });

        app.MapPut("api/services/{id}", [Authorize(Policy = "ServiceAccess")] async (long id, [FromBody] ServiceDto request, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var command = new UpdateServiceCommand(id, request);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred updating service (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update a Service", "Update a Service") { Tags = new[] { "Services" } });
    }
}
