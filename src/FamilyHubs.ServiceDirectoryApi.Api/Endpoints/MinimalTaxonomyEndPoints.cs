using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using fh_service_directory_api.api.Commands.CreateOpenReferralTaxonomy;
using fh_service_directory_api.api.Queries.GetOpenReferralTaxonomies;
using fh_service_directory_api.api.Commands.UpdateOpenReferralTaxonomy;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using AutoMapper;

namespace fh_service_directory_api.api.Endpoints;

public class MinimalTaxonomyEndPoints
{
    public void RegisterTaxonomyEndPoints(WebApplication app)
    {
        app.MapPost("api/taxonomies", async ([FromBody] OpenReferralTaxonomyDto request, CancellationToken cancellationToken, ISender _mediator) =>
        {
            try
            {
                CreateOpenReferralTaxonomyCommand command = new(request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Taxonomy", "Create Taxonomy") { Tags = new[] { "Taxonomies" } });

        app.MapPut("api/taxonomies/{id}", async (string id, [FromBody] OpenReferralTaxonomyDto request, CancellationToken cancellationToken, ISender _mediator, IMapper mapper) =>
        {
            try
            {
                UpdateOpenReferralTaxonomyCommand command = new(id, request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update Taxonomy", "Update Taxonomy") { Tags = new[] { "Taxonomies" } });

        app.MapGet("api/taxonomies", async (int? pageNumber, int? pageSize, string? text, CancellationToken cancellationToken, ISender _mediator) =>
        {
            try
            {
                GetOpenReferralTaxonomiesCommand command = new(pageNumber, pageSize, text);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get All Taxonomies", "Get All Taxonomies") { Tags = new[] { "Taxonomies" } });
    }
}
