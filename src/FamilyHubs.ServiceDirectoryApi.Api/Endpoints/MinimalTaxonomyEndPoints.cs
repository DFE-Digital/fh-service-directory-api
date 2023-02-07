﻿using System.Diagnostics;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateTaxonomy;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateTaxonomy;
using FamilyHubs.ServiceDirectory.Api.Queries.GetTaxonomies;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FamilyHubs.ServiceDirectory.Api.Endpoints;

public class MinimalTaxonomyEndPoints
{
    public void RegisterTaxonomyEndPoints(WebApplication app)
    {
        app.MapPost("api/taxonomies", [Authorize(Policy = "ServiceAccess")] async ([FromBody] TaxonomyDto request, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalTaxonomyEndPoints> logger) =>
        {
            try
            {
                var command = new CreateTaxonomyCommand(request);
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

        app.MapPut("api/taxonomies/{id}", [Authorize(Policy = "ServiceAccess")] async (string id, [FromBody] TaxonomyDto request, CancellationToken cancellationToken, ISender _mediator, IMapper mapper, ILogger<MinimalTaxonomyEndPoints> logger) =>
        {
            try
            {
                var command = new UpdateTaxonomyCommand(id, request);
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
                var command = new GetTaxonomiesCommand(pageNumber, pageSize, text);
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
