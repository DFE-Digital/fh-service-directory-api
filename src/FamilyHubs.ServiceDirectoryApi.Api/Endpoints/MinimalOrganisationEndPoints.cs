using System.Diagnostics;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;
using fh_service_directory_api.api.Commands.UpdateOpenReferralOrganisation;
using fh_service_directory_api.api.Queries.GetOpenReferralOrganisationById;
using fh_service_directory_api.api.Queries.GetOrganisationAdminByOrganisationId;
using fh_service_directory_api.api.Queries.GetOrganisationTypes;
using fh_service_directory_api.api.Queries.ListOrganisation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace fh_service_directory_api.api.Endpoints;

public class MinimalOrganisationEndPoints
{
    public void RegisterOrganisationEndPoints(WebApplication app)
    {
        app.MapPost("api/organizations", [Authorize(Policy = "OrgAccess")] async ([FromBody] OpenReferralOrganisationWithServicesDto request, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                CreateOpenReferralOrganisationCommand command = new(request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating organisation (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Organisations", "Create Organisation") { Tags = new[] { "Organisations" } });

        app.MapGet("api/organizations/{id}", async (string id, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                GetOpenReferralOrganisationByIdCommand request = new()
                {
                    Id = id
                };
                var result = await _mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting organisation (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Organisation", "Get Organisation By Id") { Tags = new[] { "Organisations" } });

        app.MapGet("api/organizations", async (CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                ListOpenReferralOrganisationCommand request = new();
                var result = await _mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred listing organisation (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("List Organisations", "List Organisations") { Tags = new[] { "Organisations" } });

        app.MapPut("api/organizations/{id}", [Authorize(Policy = "AllAdminAccess")] async (string id, [FromBody] OpenReferralOrganisationWithServicesDto request, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                UpdateOpenReferralOrganisationCommand command = new(id, request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred updating organisation (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update Organisation", "Update Organisation By Id") { Tags = new[] { "Organisations" } });

        app.MapGet("api/organizationtypes", async (CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                GetOrganisationTypesCommand request = new();
                var result = await _mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred listing organisation types (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("List Organisation types", "List Organisation types") { Tags = new[] { "Organisations" } });

        app.MapGet("api/organizationAdminCode/{id}", async (string id, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                GetOrganisationAdminByOrganisationIdCommand request = new(id);
                var result = await _mediator.Send(request, cancellationToken);
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
