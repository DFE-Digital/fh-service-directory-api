using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using FamilyHubs.ServiceDirectoryApi.Api.Commands.CreateOpenReferralOrganisation;
using FamilyHubs.ServiceDirectoryApi.Api.Commands.UpdateOpenReferralOrganisation;
using FamilyHubs.ServiceDirectoryApi.Api.Queries.GetOpenReferralOrganisationById;
using FamilyHubs.ServiceDirectoryApi.Api.Queries.ListOpenReferralOrganisation;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralOrganisations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FamilyHubs.ServiceDirectoryApi.Api.Endpoints;

public class MinimalOrganisationEndPoints
{
    public void RegisterOrganisationEndPoints(WebApplication app)
    {
        app.MapPost("api/organizations", async ([FromBody] IOpenReferralOrganisationWithServicesDto request, CancellationToken cancellationToken, ISender _mediator) =>
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
                GetOpenReferralOrganisationByIdQuery request = new()
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
                ListOpenReferralOrganisationQuery request = new();
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
