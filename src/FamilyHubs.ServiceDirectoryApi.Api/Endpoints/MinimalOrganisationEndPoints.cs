using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;
using fh_service_directory_api.api.Commands.UpdateOpenReferralOrganisation;
using fh_service_directory_api.api.Queries.GetOpenReferralOrganisationById;
using fh_service_directory_api.api.Queries.ListOrganisation;
using fh_service_directory_api.core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace fh_service_directory_api.api.Endpoints;

public class MinimalOrganisationEndPoints
{
    public void RegisterOrganisationEndPoints(WebApplication app)
    {
        app.MapPost("api/organizations", async ([FromBody] OpenReferralOrganisationWithServicesDto request, CancellationToken cancellationToken, ISender _mediator) =>
        {
            try
            {
                CreateOpenReferralOrganisationCommand command = new(request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Organisations", "Create Organisation") { Tags = new[] { "Organisations" } });

        app.MapGet("api/organizations/{id}", async (string id, CancellationToken cancellationToken, ISender _mediator) =>
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
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Organisation", "Get Organisation By Id") { Tags = new[] { "Organisations" } });

        app.MapGet("api/organizations", async (CancellationToken cancellationToken, ISender _mediator) =>
        {
            try
            {
                ListOpenReferralOrganisationCommand request = new();
                var result = await _mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("List Organisations", "List Organisations") { Tags = new[] { "Organisations" } });

        app.MapPut("api/organizations/{id}", async (string id, [FromBody] OpenReferralOrganisationWithServicesDto request, CancellationToken cancellationToken, ISender _mediator, IMapper mapper) =>
        {
            try
            {
                OpenReferralOrganisation openReferralOrganisation = mapper.Map<OpenReferralOrganisation>(request);
                UpdateOpenReferralOrganisationCommand command = new(id, openReferralOrganisation);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update Organisation", "Update Organisation") { Tags = new[] { "Organisations" } });
    }
}
