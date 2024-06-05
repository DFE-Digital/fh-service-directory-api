using System.Collections;

namespace FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Models;

public class ServiceSearchRequest
{
    public string searchPostcode { get; set; }

    public int searchRadiusMiles { get; set;}

    public int userId { get; set;}

    public int httpResponseCode { get; set;}

    public DateTime requestTimestamp { get; set;}

    public DateTime responseTimestamp { get; set;}

    public string correlationId { get; set;}

    public int searchTriggerEventId { get; set;}

    public int serviceSearchTypeId { get; set;}

    public List<ServiceSearchResults> serviceSearchResults { get; set;}

}