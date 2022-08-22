//using Application.Commands.CreateOpenReferralOrganisation;
//using fh_service_directory_api.core.Interfaces.Entities;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using Swashbuckle.AspNetCore.Annotations;

//namespace fh_service_directory_api.api.Endpoints.OpenReferralOrganisation;

//public class OpenReferralOrganisationEndPoints
//{
//    public void RegisterOpenReferralOrganisationEndPoints(WebApplication application)
//    {
//        application.MapPost
//        (
//            "api/organizations", 
//            async ([FromBody] IOpenReferralOrganisation request,
//            CancellationToken cancellationToken,
//            ISender _mediator
//        ) =>
//        {
//            try
//            {
//                CreateOpenReferralOrganisationCommand command = new(request);
//                var result = await _mediator.Send(command, cancellationToken);
//                return result;
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine(ex.Message);
//                throw;
//            }
//        }).WithMetadata(new SwaggerOperationAttribute("OpenReferralOrganisations", "Create OpenReferralOrganisation") { Tags = new[] { "OpenReferralOrganisations" } });

//        //application.MapGet("api/organizations/{id}", async (string id, CancellationToken cancellationToken, ISender _mediator) =>
//        //{
//        //    try
//        //    {
//        //        GetOpenReferralOpenReferralOrganisationByIdCommand request = new()
//        //        {
//        //            Id = id
//        //        };
//        //        var result = await _mediator.Send(request, cancellationToken);
//        //        return result;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        System.Diagnostics.Debug.WriteLine(ex.Message);
//        //        throw;
//        //    }
//        //}).WithMetadata(new SwaggerOperationAttribute("Get OpenReferralOrganisation", "Get OpenReferralOrganisation By Id") { Tags = new[] { "OpenReferralOrganisations" } });

//        //application.MapGet("api/organizations", async (CancellationToken cancellationToken, ISender _mediator) =>
//        //{
//        //    try
//        //    {
//        //        ListOpenReferralOpenReferralOrganisationCommand request = new();
//        //        var result = await _mediator.Send(request, cancellationToken);
//        //        return result;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        System.Diagnostics.Debug.WriteLine(ex.Message);
//        //        throw;
//        //    }
//        //}).WithMetadata(new SwaggerOperationAttribute("List OpenReferralOrganisations", "List OpenReferralOrganisations") { Tags = new[] { "OpenReferralOrganisations" } });

//        //application.MapPut("api/organizations/{id}", async (string id, [FromBody] OpenReferralOpenReferralOrganisationWithServicesRecord request, CancellationToken cancellationToken, ISender _mediator, IMapper mapper) =>
//        //{
//        //    try
//        //    {
//        //        OpenReferralOpenReferralOrganisation openReferralOpenReferralOrganisation = mapper.Map<OpenReferralOpenReferralOrganisation>(request);
//        //        UpdateOpenReferralOpenReferralOrganisationCommand command = new(id, openReferralOpenReferralOrganisation);
//        //        var result = await _mediator.Send(command, cancellationToken);
//        //        return result;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        System.Diagnostics.Debug.WriteLine(ex.Message);
//        //        throw;
//        //    }
//        //}).WithMetadata(new SwaggerOperationAttribute("Update OpenReferralOrganisation", "Update OpenReferralOrganisation") { Tags = new[] { "OpenReferralOrganisations" } });
//    }
//}






////public class Create : EndpointBaseAsync
////    .WithRequest<Create>
////    .WithResult<ActionResult<string>>
////{
////    private ISender _mediator = null!;
////    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

////    [SwaggerOperation(
////        Summary = "Creates OrOpenReferralOrganisation",
////        Description = "Creates an OrOpenReferralOrganisation with Services",
////        OperationId = "OrOpenReferralOrganisation.Create",
////        Tags = new[] { "Simple OrOpenReferralOrganisation" })
////    ]
////    [HttpPost]
////    [Route("api/CreateMyOrOpenReferralOrganisationDepricated")]
////    public override async Task<ActionResult<string>> HandleAsync([FromBody] Create request, CancellationToken cancellationToken = default)
////    {

////        try
////        {
////            var result = await Mediator.Send(request, cancellationToken);
////            if (result == null)
////            {
////                return BadRequest();
////            }

////            return Ok(result);

////        }
////        catch (Exception ex)
////        {
////            System.Diagnostics.Debug.WriteLine(ex.Message);
////            return BadRequest();
////        }
////    }
////}

