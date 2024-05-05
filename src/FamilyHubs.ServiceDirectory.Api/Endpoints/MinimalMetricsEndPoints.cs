namespace FamilyHubs.ServiceDirectory.Api.Endpoints;

using FamilyHubs.ServiceDirectory.Core.Commands.Metrics.RecordServiceSearch;
using FamilyHubs.ServiceDirectory.Shared.Dto.Metrics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class MinimalMetricsEndPoints
{
    public void RegisterMetricsEndPoints(WebApplication app)
    {
        app.MapPost("api/metrics/service-search", async (
            [FromBody] ServiceSearchDto serviceSearch,
            CancellationToken cancellationToken,
            ISender mediator,
            ILogger<MinimalMetricsEndPoints> logger
        ) =>
        {

            try
            {
                var command = new RecordServiceSearchCommand(serviceSearch);
                long result = await mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred reporting metric data (api). {exceptionMessage}", ex.Message);
                throw;
            }
        }).WithMetadata(
            new SwaggerOperationAttribute("Record Service Search", "Records a service search transaction by a user.")
            {
                Tags = new[] { "Metrics" }
            }
        );
    }
}
