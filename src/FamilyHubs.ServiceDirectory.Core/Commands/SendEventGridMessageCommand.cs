using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.Extensions.Configuration;
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

    public bool IsUnitTesting { get; set; }
    public SendEventGridMessageCommandHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<string> Handle(SendEventGridMessageCommand request, CancellationToken cancellationToken)
    {
        if (IsRunningLocallyAndNotUnitTesting())
        {
            return "Event Grid Notifications are unavailable when running loacally";
        }
        var eventNew = await MakeRequestEvent(request);
        return eventNew.Content.ReadAsStringAsync().Result;
    }

    private async Task<HttpResponseMessage> MakeRequestEvent(SendEventGridMessageCommand request)
    {
        var eventData = new[]
        {
            new
            {
                Id = Guid.NewGuid(),
                EventType = "OrganisationDto",
                Subject = "Unit Test",
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

        Console.WriteLine(jsonContent);

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
