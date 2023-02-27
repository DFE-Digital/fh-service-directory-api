using FamilyHubs.ServiceDirectory.Api.Commands.UpsertContacts;
using FamilyHubs.ServiceDirectory.Api.Queries.GetContacts;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FamilyHubs.ServiceDirectory.Api.Endpoints;

public class MinimalContactsEndPoints
{
    public void RegisterContactsEndPoints(WebApplication app)
    {
        app.MapPost("api/contact", [Authorize(Policy = "ServiceAccess")] async ([FromBody] ContactDto request, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalContactsEndPoints> logger) =>
        {
            var command = new UpsertContactCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            if (result.Succeeded)
            {
                return Results.Ok(result.Value);
            }

            return Results.BadRequest(result.Errors);

        }).WithMetadata(new SwaggerOperationAttribute("Contact", "Create Contact") { Tags = new[] { "Contacts" } });

        app.MapGet("api/contact", async (string? id, string? title, string? name, string? telephone, string? textPhone, string? url, string? email, int? pageNumber, int? pageSize, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalContactsEndPoints> logger) =>
        {
            var command = new GetContactsCommand(id, title, name, telephone, textPhone, url, email, pageNumber,  pageSize);
            var result = await _mediator.Send(command, cancellationToken);

            if (result.Succeeded)
            {
                return Results.Ok(result.Value);
            }
            return Results.BadRequest(result.Errors);

        }).WithMetadata(new SwaggerOperationAttribute("List Contacts", "List Contacts") { Tags = new[] { "Contacts" } });
    }
}
