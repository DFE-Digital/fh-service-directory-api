using fh_service_directory_api.core.Entities;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Builders;

public class OrganisationBuilder
{
    private OpenReferralOrganisation _openReferralOrganisation;

    public OrganisationBuilder()
    {
        _openReferralOrganisation = new OpenReferralOrganisation();
    }

    public OrganisationBuilder WithMainProperties(string id, string name, string? description, string? logo, string? uri, string? url)
    {
        _openReferralOrganisation.Id = id;
        _openReferralOrganisation.Name = name;
        _openReferralOrganisation.Description = description;
        _openReferralOrganisation.Logo = logo;
        _openReferralOrganisation.Uri = uri;
        _openReferralOrganisation.Url = url;
        return this;
    }

    public OrganisationBuilder WithServices(ICollection<OpenReferralService>? Services)
    {
        _openReferralOrganisation.Services = Services;
        return this;
    }

    public OpenReferralOrganisation Build()
    {
        return _openReferralOrganisation;
    }
}
