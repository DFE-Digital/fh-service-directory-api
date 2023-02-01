using System.Diagnostics;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateUiCache;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateUiCache;
using FamilyHubs.ServiceDirectory.Api.Queries.GetUiCacheById;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FamilyHubs.ServiceDirectory.Api.Endpoints;

public class MinimalUiCacheEndPoints
{
    public void RegisterUiCacheEndPoints(WebApplication app)
    {
        app.MapPost("api/uicaches", [Authorize(Policy = "ServiceAccess")] async ([FromBody] UICacheDto request, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalUiCacheEndPoints> logger) =>
        {
            try
            {
                var command = new CreateUiCacheCommand(request);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating UICache. {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("UICaches", "Create UICache") { Tags = new[] { "UICaches" } });

        app.MapGet("api/uicaches/{id}", async (string id, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalUiCacheEndPoints> logger) =>
        {
            try
            {
                var request = new GetUiCacheByIdCommand(id);
                var result = await mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting UICache by Id. {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get UICache", "Get UICache By Id") { Tags = new[] { "UICaches" } });

        app.MapPut("api/uicaches/{id}", [Authorize(Policy = "ServiceAccess")] async (string id, [FromBody] UICacheDto request, CancellationToken cancellationToken, ISender mediator, ILogger<MinimalUiCacheEndPoints> logger) =>
        {
            try
            {
                var command = new UpdateUiCacheCommand(id, request);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred updating UICache. {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update UICache", "Update UICache By Id") { Tags = new[] { "UICaches" } });
    }
}
