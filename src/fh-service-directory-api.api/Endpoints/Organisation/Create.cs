using Ardalis.ApiEndpoints;
using fh_service_directory_api.core.Concretions.Features.Organisations.Commands.Create;
using fh_service_directory_api.core.Interfaces.Entities.Aggregates;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace fh_service_directory_api.api.Endpoints.Organisation;

public class OrganisationEndPoints
{
    public void RegisterOrganisationEndPoints(WebApplication application)
    {
        application.MapPost
        (
            "api/organizations", 
            async ([FromBody] IOrganisation request,
            CancellationToken cancellationToken,
            ISender _mediator
        ) =>
        {
            try
            {
                CreateOrganisationCommand command = new(request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Organisations", "Create Organisation") { Tags = new[] { "Organisations" } });

        //application.MapGet("api/organizations/{id}", async (string id, CancellationToken cancellationToken, ISender _mediator) =>
        //{
        //    try
        //    {
        //        GetOpenReferralOrganisationByIdCommand request = new()
        //        {
        //            Id = id
        //        };
        //        var result = await _mediator.Send(request, cancellationToken);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.Message);
        //        throw;
        //    }
        //}).WithMetadata(new SwaggerOperationAttribute("Get Organisation", "Get Organisation By Id") { Tags = new[] { "Organisations" } });

        //application.MapGet("api/organizations", async (CancellationToken cancellationToken, ISender _mediator) =>
        //{
        //    try
        //    {
        //        ListOpenReferralOrganisationCommand request = new();
        //        var result = await _mediator.Send(request, cancellationToken);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.Message);
        //        throw;
        //    }
        //}).WithMetadata(new SwaggerOperationAttribute("List Organisations", "List Organisations") { Tags = new[] { "Organisations" } });

        //application.MapPut("api/organizations/{id}", async (string id, [FromBody] OpenReferralOrganisationWithServicesRecord request, CancellationToken cancellationToken, ISender _mediator, IMapper mapper) =>
        //{
        //    try
        //    {
        //        OpenReferralOrganisation openReferralOrganisation = mapper.Map<OpenReferralOrganisation>(request);
        //        UpdateOpenReferralOrganisationCommand command = new(id, openReferralOrganisation);
        //        var result = await _mediator.Send(command, cancellationToken);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.Message);
        //        throw;
        //    }
        //}).WithMetadata(new SwaggerOperationAttribute("Update Organisation", "Update Organisation") { Tags = new[] { "Organisations" } });
    }
}






//public class Create : EndpointBaseAsync
//    .WithRequest<Create>
//    .WithResult<ActionResult<string>>
//{
//    private ISender _mediator = null!;
//    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

//    [SwaggerOperation(
//        Summary = "Creates OrOrganisation",
//        Description = "Creates an OrOrganisation with Services",
//        OperationId = "OrOrganisation.Create",
//        Tags = new[] { "Simple OrOrganisation" })
//    ]
//    [HttpPost]
//    [Route("api/CreateMyOrOrganisationDepricated")]
//    public override async Task<ActionResult<string>> HandleAsync([FromBody] Create request, CancellationToken cancellationToken = default)
//    {

//        try
//        {
//            var result = await Mediator.Send(request, cancellationToken);
//            if (result == null)
//            {
//                return BadRequest();
//            }

//            return Ok(result);

//        }
//        catch (Exception ex)
//        {
//            System.Diagnostics.Debug.WriteLine(ex.Message);
//            return BadRequest();
//        }
//    }
//}

