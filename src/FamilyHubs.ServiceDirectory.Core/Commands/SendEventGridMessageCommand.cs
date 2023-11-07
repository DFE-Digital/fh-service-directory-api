﻿using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace FamilyHubs.ServiceDirectory.Core.Commands;

public class SendEventGridMessageCommand : IRequest<string>
{
    public SendEventGridMessageCommand(OrganisationDto organisationDto)
    {
        OrganisationDto = organisationDto;
    }

    public OrganisationDto OrganisationDto { get; set; }
}

public class SendEventGridMessageCommandHandler : IRequestHandler<SendEventGridMessageCommand, string>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SendEventGridMessageCommandHandler> _logger;

    public bool IsUnitTesting { get; set; }
    public SendEventGridMessageCommandHandler(IConfiguration configuration, ILogger<SendEventGridMessageCommandHandler> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    public async Task<string> Handle(SendEventGridMessageCommand request, CancellationToken cancellationToken)
    {
        if (IsRunningLocallyAndNotUnitTesting())
        {
            _logger.LogWarning("Event Grid Notifications are unavailable when running locally");
            return "Event Grid Notifications are unavailable when running locally";
        }
        var eventNew = await MakeRequestEvent(request);
        return eventNew.Content.ReadAsStringAsync().Result;
    }

    private async Task<HttpResponseMessage> MakeRequestEvent(SendEventGridMessageCommand request)
    {
        _logger.LogInformation("Createing Organisation Event Grid Notification");
        var eventData = new[]
        {
            new
            {
                Id = Guid.NewGuid(),
                EventType = "OrganisationDto",
                Subject = "Organisation",
                EventTime = DateTime.UtcNow,
                Data = request.OrganisationDto
            }
        };

        string? endpoint = _configuration["EventGridUrl"];
        string? aegsaskey = _configuration["aeg-sas-key"];

        if (string.IsNullOrEmpty(endpoint))
        {
            throw new ArgumentException("EventGridUrl is missing");
        }
        if (string.IsNullOrEmpty(aegsaskey))
        {
            throw new ArgumentException("aeg-sas-key");
        }

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("aeg-sas-key", aegsaskey);

        string jsonContent = JsonConvert.SerializeObject(eventData);

        _logger.LogInformation($"Sending Grid Event Payload: {jsonContent}");

        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        return await httpClient.PostAsync(endpoint, content);
    }

    private bool IsRunningLocallyAndNotUnitTesting()
    {
        if (IsUnitTesting)
            return false;

        try
        {
            string localMachineName = _configuration["LocalSettings:MachineName"] ?? string.Empty;

            if (!string.IsNullOrEmpty(localMachineName))
            {
                return Environment.MachineName.Equals(localMachineName, StringComparison.OrdinalIgnoreCase);
            }
        }
        catch
        {
            return false;
        }

        // Fallback to a default check if User Secrets file or machine name is not specified
        // For example, you can add additional checks or default behavior here
        return false;
    }
}