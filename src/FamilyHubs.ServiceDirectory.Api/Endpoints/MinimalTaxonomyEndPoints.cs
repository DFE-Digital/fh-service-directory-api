using FamilyHubs.ServiceDirectory.Core.Commands.Taxonomies.CreateTaxonomy;
using FamilyHubs.ServiceDirectory.Core.Commands.Taxonomies.UpdateTaxonomy;
using FamilyHubs.ServiceDirectory.Core.Queries.Taxonomies.GetTaxonomies;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FamilyHubs.ServiceDirectory.Api.Endpoints;

public class MinimalTaxonomyEndPoints
{
    public void RegisterTaxonomyEndPoints(WebApplication app)
    {
        app.MapPost("api/taxonomies",
            [Authorize(Roles = $"{RoleTypes.DfeAdmin},{RoleTypes.LaManager},{RoleTypes.LaDualRole}")] async 
            ([FromBody] TaxonomyDto request, 
            CancellationToken cancellationToken, 
            ISender mediator, 
            ILogger<MinimalTaxonomyEndPoints> logger) =>
        {
            try
            {
                var command = new CreateTaxonomyCommand(request);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating taxonomy. {ExceptionMessage}", ex.Message);
                
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Taxonomy", "Create Taxonomy") { Tags = new[] { "Taxonomies" } });

        app.MapPut("api/taxonomies/{id}",
            [Authorize(Roles = $"{RoleTypes.DfeAdmin},{RoleTypes.LaManager},{RoleTypes.LaDualRole}")] async 
            (long id, 
            [FromBody] TaxonomyDto request, 
            CancellationToken cancellationToken, 
            ISender mediator, 
            ILogger<MinimalTaxonomyEndPoints> logger) =>
        {
            try
            {
                var command = new UpdateTaxonomyCommand(id, request);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred updating taxonomy. {ExceptionMessage}", ex.Message);
                
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update Taxonomy", "Update Taxonomy") { Tags = new[] { "Taxonomies" } });

        app.MapGet("api/taxonomies", async (TaxonomyType taxonomyType, int? pageNumber, int? pageSize, string? text, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalTaxonomyEndPoints> logger) =>
        {
            try
            {
                var command = new GetTaxonomiesCommand(taxonomyType, pageNumber, pageSize, text);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting taxonomies. {ExceptionMessage}", ex.Message);
                
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get All Taxonomies", "Get All Taxonomies") { Tags = new[] { "Taxonomies" } });
    }
}
