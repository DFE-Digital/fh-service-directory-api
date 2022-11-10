﻿using fh_service_directory_api.api.Queries.FxSearch;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace fh_service_directory_api.api.Endpoints
{
    public class MinimalSearchEndPoints
    {
        public void RegisterSearchEndPoints(WebApplication app)
        {
            app.MapGet("api/search", async (string? districtCode, double? longitude, double? latitude, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalServiceEndPoints> logger) =>
            {
                try
                {
                    FxSearchCommand command = new(districtCode, latitude, longitude);
                    var result = await _mediator.Send(command, cancellationToken);
                    return result;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred listing open referral services. {exceptionMessage}", ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    throw;
                }
            }).WithMetadata(new SwaggerOperationAttribute("Search", "Search Services") { Tags = new[] { "Search Services" } });
        }
    }
}