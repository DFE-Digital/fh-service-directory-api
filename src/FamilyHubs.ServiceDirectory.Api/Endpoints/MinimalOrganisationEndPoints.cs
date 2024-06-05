using FamilyHubs.ServiceDirectory.Core.Commands.Organisations.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Core.Commands.Organisations.DeleteOrganisation;
using FamilyHubs.ServiceDirectory.Core.Commands.Organisations.UpdateOrganisation;
using FamilyHubs.ServiceDirectory.Core.Queries.Organisations.GetOrganisationAdminAreaById;
using FamilyHubs.ServiceDirectory.Core.Queries.Organisations.GetOrganisationById;
using FamilyHubs.ServiceDirectory.Core.Queries.Organisations.GetOrganisationsByAssociatedId;
using FamilyHubs.ServiceDirectory.Core.Queries.Organisations.ListOrganisations;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FamilyHubs.ServiceDirectory.Api.Endpoints;

public class MinimalOrganisationEndPoints
{
    public void RegisterOrganisationEndPoints(WebApplication app)
    {
        app.MapGet("api/organisations/{id}", async (long id, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
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
                logger.LogError(ex, "An error occurred getting organisation (api). {ExceptionMessage}", ex.Message);
                
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Organisation", "Get Organisation By Id") { Tags = new[] { "Organisations" } });

        app.MapGet("api/organisationAdminCode/{id}", async (long id, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var request = new GetOrganisationAdminAreaByIdCommand
                {
                    OrganisationId = id
                };
                var result = await mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting organisation admin code (api). {ExceptionMessage}", ex.Message);
                
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Organisation Code By Organisation Id", "Get Organisation Code By Organisation Id") { Tags = new[] { "Organisations" } });

        app.MapGet("api/organisations", async (
            [FromQuery] long[] ids,
            [FromQuery] string? name,
            [FromQuery] OrganisationType? organisationType,
            [FromQuery] long? associatedOrganisationId,
            CancellationToken cancellationToken, ISender mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var request = new ListOrganisationsCommand(ids.ToList(), name, organisationType, associatedOrganisationId);
                var result = await mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred listing organisation (api). {ExceptionMessage}", ex.Message);
                
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("List Organisations", "List Organisations") { Tags = new[] { "Organisations" } });

        app.MapPut("api/organisations/{id}",
            [Authorize(Roles = $"{RoleTypes.DfeAdmin},{RoleTypes.LaManager},{RoleTypes.LaDualRole}")] async 
            (long id, 
            [FromBody] OrganisationDetailsDto request, 
            CancellationToken cancellationToken, 
            ISender mediator, 
            ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var command = new UpdateOrganisationCommand(id, request);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred updating organisation (api). {ExceptionMessage}", ex.Message);
                
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update Organisation", "Update Organisation By Id") { Tags = new[] { "Organisations" } });

        app.MapPost("api/organisations", 
            [Authorize(Roles = $"{RoleTypes.DfeAdmin},{RoleTypes.LaManager},{RoleTypes.LaDualRole}")] async 
            ([FromBody] OrganisationDetailsDto request, 
            CancellationToken cancellationToken, 
            ISender mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var command = new CreateOrganisationCommand(request);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating organisation (api). {ExceptionMessage}", ex.Message);
                
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Organisations", "Create Organisation") { Tags = new[] { "Organisations" } });

        app.MapGet("api/organisationsByAssociatedOrganisation", async (long id, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                var request = new GetOrganisationsByAssociatedIdCommand(id);
                var result = await mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred listing organisation (api). {ExceptionMessage}", ex.Message);

                throw;
            }
        }).WithMetadata(
            new SwaggerOperationAttribute(
                "List Organisations By Parent", 
                "Lists Organisations associated with the parent id, also returns parent organisation"
                ) { Tags = new[] { "Organisations" } });

        app.MapDelete("api/organisations/{id}",
            [Authorize(Roles = $"{RoleTypes.DfeAdmin},{RoleTypes.LaManager},{RoleTypes.LaDualRole}")] async
            (long id,            
            CancellationToken cancellationToken,
            ISender mediator,
            ILogger<MinimalOrganisationEndPoints> logger) =>
            {
                try
                {
                    var command = new DeleteOrganisationCommand(id);
                    var result = await mediator.Send(command, cancellationToken);
                    return result;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred deleting organisation (api). {ExceptionMessage}", ex.Message);

                    throw;
                }
            }).WithMetadata(new SwaggerOperationAttribute("Delete Organisation", "Delete Organisation By Id") { Tags = new[] { "Organisations" } });

    }
}
