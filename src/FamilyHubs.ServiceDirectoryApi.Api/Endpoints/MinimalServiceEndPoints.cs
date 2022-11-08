using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using fh_service_directory_api.api.Commands.CreateOpenReferralService;
using fh_service_directory_api.api.Commands.DeleteOpenReferralService;
using fh_service_directory_api.api.Commands.UpdateOpenReferralService;
using fh_service_directory_api.api.Queries.GetOpenReferralService;
using fh_service_directory_api.api.Queries.GetOpenReferralServicesByOrganisation;
using fh_service_directory_api.api.Queries.GetServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace fh_service_directory_api.api.Endpoints;

public class MinimalServiceEndPoints
{
    public void RegisterServiceEndPoints(WebApplication app)
    {
        app.MapGet("api/services", async (string? status, string ? districtCode, int ? minimum_age, int? maximum_age, int? given_age, double? latitude, double? longtitude, double? proximity, int? pageNumber, int? pageSize, string? text, string? serviceDeliveries, bool? isPaidFor, string? taxonmyIds, string ? languages, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalServiceEndPoints> logger) =>
        {
            try
            {
                GetOpenReferralServicesCommand command = new(status, districtCode, minimum_age, maximum_age, given_age, latitude, longtitude, proximity, pageNumber, pageSize, text, serviceDeliveries, isPaidFor, taxonmyIds, languages);
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

        app.MapDelete("api/services/{id}", async (string id, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalServiceEndPoints> logger) =>
        {
            try
            {
                DeleteOpenReferralServiceByIdCommand command = new(id);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred deleting open referral service by id. {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Delete Service by Id", "Delete Service by Id") { Tags = new[] { "Services" } });

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

        app.MapPost("api/services", async ([FromBody] OpenReferralServiceDto request, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                CreateOpenReferralServiceCommand command = new(request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating service (api). {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Create a Service", "Create a Service") { Tags = new[] { "Services" } });

        app.MapPut("api/services/{id}", async (string id, [FromBody] OpenReferralServiceDto request, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalOrganisationEndPoints> logger) =>
        {
            try
            {
                UpdateOpenReferralServiceCommand command = new(id, request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred updating service (api). {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update a Service", "Update a Service") { Tags = new[] { "Services" } });
    }



}
