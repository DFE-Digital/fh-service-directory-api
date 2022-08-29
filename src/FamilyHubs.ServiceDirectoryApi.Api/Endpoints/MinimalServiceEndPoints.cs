using FamilyHubs.ServiceDirectoryApi.Api.Queries.GetOpenReferralService;
using FamilyHubs.ServiceDirectoryApi.Api.Queries.GetOpenReferralServicesByOrganisation;
using FamilyHubs.ServiceDirectoryApi.Api.Queries.GetServices;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FamilyHubs.ServiceDirectoryApi.Api.Endpoints;

public class MinimalServiceEndPoints
{
    public void RegisterServiceEndPoints(WebApplication app)
    {
        //Http://localhost:portnumber/latitude/longtitude/meters?pageNumber=1&pageSize=10
        //string[]? taxonomy_id, string[]? taxonomy_type, string[]? vocabulary,
        app.MapGet("api/services", async (string? status, int? minimum_age, int? maximum_age, double? latitude, double? longtitude, double? proximity, int? pageNumber, int? pageSize, string? text, CancellationToken cancellationToken, ISender _mediator) =>
        {
            try
            {
                GetOpenReferralServicesQuery command = new(status, minimum_age, maximum_age, latitude, longtitude, proximity, pageNumber, pageSize, text);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("List Services", "List Services") { Tags = new[] { "Services" } });

        app.MapGet("api/services/{id}", async (string id, CancellationToken cancellationToken, ISender _mediator) =>
        {
            try
            {
                GetOpenReferralServiceByIdQuery command = new(id);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Service by Id", "Get Service by Id") { Tags = new[] { "Services" } });

        app.MapGet("api/organisationservices/{id}", async (string id, CancellationToken cancellationToken, ISender _mediator) =>
        {
            try
            {
                GetOpenReferralServicesByOrganisationIdQuery command = new(id);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Services by Organisation Id", "Get Service by Organisation Id") { Tags = new[] { "Services" } });
    }

}
