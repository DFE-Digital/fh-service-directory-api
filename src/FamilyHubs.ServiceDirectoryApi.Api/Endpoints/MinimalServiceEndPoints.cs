using fh_service_directory_api.api.Queries.GetOpenReferralService;
using fh_service_directory_api.api.Queries.GetOpenReferralServicesByOrganisation;
using fh_service_directory_api.api.Queries.GetServices;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace fh_service_directory_api.api.Endpoints;

public class MinimalServiceEndPoints
{
    public void RegisterServiceEndPoints(WebApplication app)
    {
        app.MapGet("api/services", async (string? status, int? minimum_age, int? maximum_age, double? latitude, double? longtitude, double? proximity, int? pageNumber, int? pageSize, string? text, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalServiceEndPoints> logger) =>
        {
            try
            {
                GetOpenReferralServicesCommand command = new(status, minimum_age, maximum_age, latitude, longtitude, proximity, pageNumber, pageSize, text);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred listing open referral services. {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("List Services", "List Services") { Tags = new[] { "Services" } });

        app.MapGet("api/services/{id}", async (string id, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalServiceEndPoints> logger) =>
        {
            try
            {
                GetOpenReferralServiceByIdCommand command = new(id);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting open referral service by id. {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Service by Id", "Get Service by Id") { Tags = new[] { "Services" } });

        app.MapGet("api/organisationservices/{id}", async (string id, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalServiceEndPoints> logger) =>
        {
            try
            {
                GetOpenReferralServicesByOrganisationIdCommand command = new(id);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting open referral service by id. {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Services by Organisation Id", "Get Service by Organisation Id") { Tags = new[] { "Services" } });
    }

}
