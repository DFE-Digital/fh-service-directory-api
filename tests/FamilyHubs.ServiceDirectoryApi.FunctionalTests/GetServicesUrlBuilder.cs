namespace FamilyHubs.ServiceDirectoryApi.FunctionalTests;

public class GetServicesUrlBuilder
{
    private readonly List<string> _urlParameter = new();

    public GetServicesUrlBuilder WithServiceType(string serviceType)
    {
        _urlParameter.Add($"serviceType={serviceType}");
        return this;
    }
    public GetServicesUrlBuilder WithStatus(string status)
    {
        _urlParameter.Add($"status={status}");
        return this;
    }

    public GetServicesUrlBuilder WithDistrictCode(string code)
    {
        _urlParameter.Add($"districtCode={code}");
        return this;
    }

    public GetServicesUrlBuilder WithEligibility(int minimum_age, int maximum_age)
    {
        _urlParameter.Add( $"minimum_age={minimum_age}&maximum_age={maximum_age}" );
        return this;
    }

    public GetServicesUrlBuilder WithProximity(double latitude, double longtitude, double proximity)
    {
        _urlParameter.Add($"latitude={latitude}&longtitude={longtitude}&proximity={proximity}");
        return this;
    }

    public GetServicesUrlBuilder WithPage(int pageNumber, int pageSize)
    {
        _urlParameter.Add($"pageNumber={pageNumber}&pageSize={pageSize}");
        return this;
    }

    public GetServicesUrlBuilder WithSearchText(string searchText)
    {
        _urlParameter.Add($"text={searchText}");
        return this;
    }

    public GetServicesUrlBuilder WithDelimitedSearchDeliveries(string serviceDeliveries)
    {
        _urlParameter.Add($"serviceDeliveries={serviceDeliveries}");
        return this;
    }

    public GetServicesUrlBuilder WithDelimitedTaxonomies(string taxonmyIds)
    {
        _urlParameter.Add($"taxonmyIds={taxonmyIds}");
        return this;
    }

    public string Build()
    {
        bool isFirst = true;
        string url = string.Empty;
        foreach(string param in _urlParameter)
        {
            if (isFirst)
            {
                isFirst = false;
                url += "?" + param;
            }          
            else
                url += "&" + param;
        }

        return url;
    }
}
