using FamilyHubs.ServiceDirectory.Core.Commands.Services.CreateService;
using FamilyHubs.ServiceDirectory.Core.Commands.Services.DeleteService;
using FamilyHubs.ServiceDirectory.Core.Commands.Services.UpdateService;
using FamilyHubs.ServiceDirectory.Core.Queries.Services.GetServiceById;
using FamilyHubs.ServiceDirectory.Core.Queries.Services.GetServiceByOwnerReferenceIdCommand;
using FamilyHubs.ServiceDirectory.Core.Queries.Services.GetServices;
using FamilyHubs.ServiceDirectory.Core.Queries.Services.GetServicesByOrganisationId;
using FamilyHubs.ServiceDirectory.Shared.Dto;
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
        app.MapGet("api/services-simple", async (ServiceType? serviceType, ServiceStatusType? status, string? districtCode, int? minimumAge, int? maximumAge, int? givenAge, double? latitude, double? longitude, double? proximity, int? pageNumber, int? pageSize, string? text, string? serviceDeliveries, bool? isPaidFor, string? taxonomyIds, string? languages, bool? canFamilyChooseLocation, bool? isFamilyHub, int? maxFamilyHubs, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalServiceEndPoints> logger) =>
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

                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("List Services", "List Services") { Tags = new[] { "Services" } });

        //todo: this should be enough to get the name, but is it enough to update the service?
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
                logger.LogError(ex, "An error occurred getting open referral service by id. {exceptionMessage}", ex.Message);
                
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
                logger.LogError(ex, "An error occurred getting open referral service by id. {exceptionMessage}", ex.Message);

                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Service by Id", "Get Service by Id") { Tags = new[] { "Services" } });

        app.MapGet("api/servicesByOwnerReference/{ownerReferenceId}", async (string ownerReferenceId, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalServiceEndPoints> logger) =>
        {
            try
            {
                var command = new GetServiceByOwnerReferenceIdCommand
                {
                    OwnerReferenceId = ownerReferenceId
                };
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting open referral service by OwnerReferenceId. {exceptionMessage}", ex.Message);
                
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Service by OwnerReferenceId", "Get Service by OwnerReferenceId") { Tags = new[] { "Services" } });

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

        //todo: it would be much more efficient to use patch, rather than update
        // there are some hoops to jump through to get JsonPatchDocument working with minimal apis though
        // (see https://learn.microsoft.com/en-us/answers/questions/1348193/jsonpatchdocument-problems-with-minimal-api-in-net)
        // and it wouldn't be particularly swagger friendly (but that's a price worth paying)
        // we could use controllers instead for the patch endpoints though
        // requires <PackageReference Include="Microsoft.AspNetCore.JsonPatch" Version="7.0.14" />

        //app.MapPatch("api/services/{id}", async (HttpRequest req, ILogger log) =>
        //{
        //    var patchDoc = await JsonSerializer.DeserializeAsync<JsonPatchDocument<ServiceDto>>(req.Body);
        //});

        //app.MapPatch("api/services/{id}", async (string id, [FromBody] JsonElement jsonElement) =>
        //{
        //    var json = jsonElement.GetRawText();

        //    var doc = JsonConvert.DeserializeObject<JsonPatchDocument>(json);
        //});

        //[HttpPatch("{id}")]
        //public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Service> patchDoc)
        //{
        //    if (patchDoc != null)
        //    {
        //        var service = await _context.Services.FindAsync(id);

        //        patchDoc.ApplyTo(service, ModelState);

        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        await _context.SaveChangesAsync();

        //        return new ObjectResult(service);
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}

        //todo: use AdminRole from updated shared kernel (need to update azure.identity first)
        //todo: check if any other consumers, as we're changing the roles here
        //todo: check with rider's ef core analyzer
        app.MapPut("api/services/{id}",
            [Authorize(Roles = $"{RoleTypes.DfeAdmin},{RoleTypes.LaManager},{RoleTypes.LaDualRole},{RoleTypes.VcsManager},{RoleTypes.VcsDualRole}")] async 
            (long id, 
            [FromBody] ServiceDto request, 
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
                logger.LogError(ex, "An error occurred updating service (api). {exceptionMessage}", ex.Message);
                
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update a Service", "Update a Service") { Tags = new[] { "Services" } });
        
        app.MapPost("api/services",
            [Authorize(Roles = $"{RoleTypes.DfeAdmin},{RoleTypes.LaManager},{RoleTypes.LaDualRole}")] async 
            ([FromBody] ServiceDto request, 
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
                logger.LogError(ex, "An error occurred creating service (api). {exceptionMessage}", ex.Message);
                
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
                logger.LogError(ex, "An error occurred deleting open referral service by id. {exceptionMessage}", ex.Message);
                
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Delete Service by Id", "Delete Service by Id") { Tags = new[] { "Services" } });
    }
}
