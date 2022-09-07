using FamilyHubs.ServiceDirectory.Shared.Models.Api;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;
using fh_service_directory_api.api.Commands.CreateUICache;
using MediatR;
using fh_service_directory_api.api.Queries.GetUICacheById;
using fh_service_directory_api.api.Commands.UpdateUICache;

namespace fh_service_directory_api.api.Endpoints;

public class MinimalUICacheEndPoints
{
    public void RegisterUICacheEndPoints(WebApplication app)
    {
        app.MapPost("api/uicaches", async ([FromBody] UICacheDto request, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalUICacheEndPoints> logger) =>
        {
            try
            {
                CreateUICacheCommand command = new(request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating UICache. {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("UICaches", "Create UICache") { Tags = new[] { "UICaches" } });

        app.MapGet("api/uicaches/{id}", async (string id, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalUICacheEndPoints> logger) =>
        {
            try
            {
                GetUICacheByIdCommand request = new(id);
                var result = await _mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting UICache by Id. {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get UICache", "Get UICache By Id") { Tags = new[] { "UICaches" } });

        app.MapPut("api/uicaches/{id}", async (string id, [FromBody] UICacheDto request, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalUICacheEndPoints> logger) =>
        {
            try
            {
                UpdateUICacheCommand command = new(id, request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred updating UICache. {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update UICache", "Update UICache By Id") { Tags = new[] { "UICaches" } });
    }
}
