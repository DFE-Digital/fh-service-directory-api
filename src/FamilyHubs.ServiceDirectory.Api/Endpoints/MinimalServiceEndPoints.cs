using FamilyHubs.ServiceDirectory.Core.Commands.Services.CreateService;
using FamilyHubs.ServiceDirectory.Core.Commands.Services.DeleteService;
using FamilyHubs.ServiceDirectory.Core.Commands.Services.UpdateService;
using FamilyHubs.ServiceDirectory.Core.Queries.Services.GetServiceById;
using FamilyHubs.ServiceDirectory.Core.Queries.Services.GetServices;
using FamilyHubs.ServiceDirectory.Core.Queries.Services.GetServicesByOrganisationId;
using FamilyHubs.ServiceDirectory.Shared.CreateUpdateDto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FamilyHubs.ServiceDirectory.Api.Endpoints;

public class MinimalServiceEndPoints
{
    public void RegisterServiceEndPoints(WebApplication app)
    {
        //todo: rename
        app.MapGet("api/services-simple", (
            ServiceType? serviceType, ServiceStatusType? status,
            string? districtCode,
            bool? allChildrenYoungPeople, int? givenAge,
            double? latitude, double? longitude, double? proximity,
            int? pageNumber, int? pageSize,
            string? text,
            string? serviceDeliveries,
            bool? isPaidFor,
            string? taxonomyIds,
            string? languages,
            bool? canFamilyChooseLocation,
            bool? isFamilyHub,
            CancellationToken cancellationToken, ISender mediator) =>
        {
            var command = new GetServicesCommand(serviceType, status, districtCode,
                allChildrenYoungPeople, givenAge, latitude, longitude, proximity, pageNumber, pageSize, text,
                serviceDeliveries, isPaidFor, taxonomyIds, languages, canFamilyChooseLocation, isFamilyHub);
            return mediator.Send(command, cancellationToken);

        }).WithMetadata(new SwaggerOperationAttribute("List Services", "List Services") { Tags = new[] { "Services" } });

        app.MapGet("api/services-simple/{id}", async (long id, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalServiceEndPoints> logger) =>
        {
            try
            {
                var command = new GetServiceByIdCommand
                {
                    Id = id,
                    IsSimple = true
                };
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting open referral service by id. {ExceptionMessage}", ex.Message);
                
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Service by Id", "Get Service by Id") { Tags = new[] { "Services" } });

        //todo: are there any other consumers? if not, change to simple (or summary)
        //todo: this looks like old code
        app.MapGet("api/services/{id}", async (long id, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalServiceEndPoints> logger) =>
        {
            try
            {
                var command = new GetServiceByIdCommand
                {
                    Id = id,
                    IsSimple = false
                };
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting open referral service by id. {ExceptionMessage}", ex.Message);

                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Service by Id", "Get Service by Id") { Tags = new[] { "Services" } });

        //todo: provide default for all params?
        app.MapGet("api/services/summary", 
            [Authorize] async (long? organisationId, string? serviceNameSearch, int pageNumber, int pageSize, SortOrder sortOrder,
                CancellationToken cancellationToken, ISender mediator, ILogger<MinimalServiceEndPoints> logger) =>
        {
            var command = new GetServiceNamesCommand
            {
                OrganisationId = organisationId,
                ServiceNameSearch = serviceNameSearch,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Order = sortOrder
            };
            return await mediator.Send(command, cancellationToken);

        }).WithMetadata(new SwaggerOperationAttribute("Get service names", "Get service names, optionally by Organisation Id") { Tags = new[] { "Services" } });

        //todo: check with rider's ef core analyzer
        app.MapPut("api/services/{id}", [Authorize(Roles = RoleGroups.AdminRole)] async 
            (long id, 
            [FromBody] ServiceChangeDto request, 
            CancellationToken cancellationToken, 
            ISender mediator, 
            ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var command = new UpdateServiceCommand(id, request);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred updating service (api). {ExceptionMessage}", ex.Message);
                
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update a Service", "Update a Service") { Tags = new[] { "Services" } });

        app.MapPost("api/services",
            [Authorize(Roles = RoleGroups.AdminRole)] async 
            ([FromBody] ServiceChangeDto request, 
            CancellationToken cancellationToken, 
            ISender mediator, 
            ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var command = new CreateServiceCommand(request);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating service (api). {ExceptionMessage}", ex.Message);
                
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Create a Service", "Create a Service") { Tags = new[] { "Services" } });
        
        app.MapDelete("api/services/{id}",
            [Authorize(Roles = $"{RoleTypes.DfeAdmin},{RoleTypes.LaManager},{RoleTypes.LaDualRole}")] async 
            (long id, 
            CancellationToken cancellationToken, 
            ISender mediator, 
            ILogger<MinimalServiceEndPoints> logger) =>
        {
            try
            {
                var command = new DeleteServiceByIdCommand(id);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred deleting open referral service by id. {ExceptionMessage}", ex.Message);
                
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Delete Service by Id", "Delete Service by Id") { Tags = new[] { "Services" } });
    }
}
