using System.Diagnostics;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using fh_service_directory_api.api.Commands.CreateOpenReferralTaxonomy;
using fh_service_directory_api.api.Commands.UpdateOpenReferralTaxonomy;
using fh_service_directory_api.api.Queries.GetOpenReferralTaxonomies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace fh_service_directory_api.api.Endpoints;

public class MinimalTaxonomyEndPoints
{
    public void RegisterTaxonomyEndPoints(WebApplication app)
    {
        app.MapPost("api/taxonomies", [Authorize(Policy = "ServiceAccess")] async ([FromBody] OpenReferralTaxonomyDto request, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalTaxonomyEndPoints> logger) =>
        {
            try
            {
                CreateOpenReferralTaxonomyCommand command = new(request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating taxonomy. {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Taxonomy", "Create Taxonomy") { Tags = new[] { "Taxonomies" } });

        app.MapPut("api/taxonomies/{id}", [Authorize(Policy = "ServiceAccess")] async (string id, [FromBody] OpenReferralTaxonomyDto request, CancellationToken cancellationToken, ISender _mediator, IMapper mapper, ILogger<MinimalTaxonomyEndPoints> logger) =>
        {
            try
            {
                UpdateOpenReferralTaxonomyCommand command = new(id, request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred updating taxonomy. {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update Taxonomy", "Update Taxonomy") { Tags = new[] { "Taxonomies" } });

        app.MapGet("api/taxonomies", async (int? pageNumber, int? pageSize, string? text, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalTaxonomyEndPoints> logger) =>
        {
            try
            {
                GetOpenReferralTaxonomiesCommand command = new(pageNumber, pageSize, text);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting taxonomies. {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get All Taxonomies", "Get All Taxonomies") { Tags = new[] { "Taxonomies" } });
    }
}
