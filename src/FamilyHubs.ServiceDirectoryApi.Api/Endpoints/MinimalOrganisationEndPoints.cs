using System.Diagnostics;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationAdminByOrganisationId;
using FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationById;
using FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationTypes;
using FamilyHubs.ServiceDirectory.Api.Queries.ListOrganisation;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FamilyHubs.ServiceDirectory.Api.Endpoints;

public class MinimalOrganisationEndPoints
{
    public void RegisterOrganisationEndPoints(WebApplication app)
    {
        app.MapPost("api/organizations", [Authorize(Policy = "OrgAccess")] async ([FromBody] OrganisationWithServicesDto request, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var command = new CreateOrganisationCommand(request);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating organisation (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Organisations", "Create Organisation") { Tags = new[] { "Organisations" } });

        app.MapGet("api/organizations/{id}", async (long id, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var request = new GetOrganisationByIdCommand
                {
                    Id = id
                };
                var result = await mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting organisation (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Organisation", "Get Organisation By Id") { Tags = new[] { "Organisations" } });

        app.MapGet("api/organizations", async (CancellationToken cancellationToken, ISender mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var request = new ListOrganisationCommand();
                var result = await mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred listing organisation (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("List Organisations", "List Organisations") { Tags = new[] { "Organisations" } });

        app.MapPut("api/organizations/{id}", [Authorize(Policy = "AllAdminAccess")] async (long id, [FromBody] OrganisationWithServicesDto request, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var command = new UpdateOrganisationCommand(id, request);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred updating organisation (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update Organisation", "Update Organisation By Id") { Tags = new[] { "Organisations" } });

        app.MapGet("api/organizationtypes", async (CancellationToken cancellationToken, ISender mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var request = new GetOrganisationTypesCommand();
                var result = await mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred listing organisation types (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("List Organisation types", "List Organisation types") { Tags = new[] { "Organisations" } });

        app.MapGet("api/organizationAdminCode/{id}", async (long id, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var request =
                    new GetOrganisationAdminByOrganisationIdCommand(id);
                var result = await mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting organisation admin code (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Organisation Code By Organisation Id", "Get Organisation Code By Organisation Id") { Tags = new[] { "Organisations" } });

    }
}
