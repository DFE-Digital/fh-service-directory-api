using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using fh_service_directory_api.api.Queries.GetPostcode;

namespace fh_service_directory_api.api.Endpoints
{
    public class MinimalPostcodeSearchEndPoints
    {
        public void RegisterServiceEndPoints(WebApplication app)
        {
            app.MapGet("api/postcode/{postcode}", async (string postcode, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalPostcodeSearchEndPoints> logger) =>
            {
                try
                {
                    GetPostcodeCommand command = new(postcode);
                    var result = await _mediator.Send(command, cancellationToken);
                    return result;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred listing postcode search services. {exceptionMessage}", ex.Message);
                    throw;
                }
            }).WithMetadata(new SwaggerOperationAttribute("Get Postcode Details", "Get Postcode") { Tags = new[] { "Utilities" } });
        }
    }
}
