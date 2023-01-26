using FamilyHubs.ServiceDirectory.Core.Entities;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Builders;

public class OrganisationBuilder
{
    private readonly ServiceDirectory.Core.Entities.Organisation _organisation;

    public OrganisationBuilder()
    {
        _organisation = new ServiceDirectory.Core.Entities.Organisation();
    }

    public OrganisationBuilder WithMainProperties(string id, string name, string? description, string? logo, string? uri, string? url)
    {
        _organisation.Id = id;
        _organisation.Name = name;
        _organisation.Description = description;
        _organisation.Logo = logo;
        _organisation.Uri = uri;
        _organisation.Url = url;
        return this;
    }

    public OrganisationBuilder WithServices(ICollection<Service>? Services)
    {
        _organisation.Services = Services;
        return this;
    }

    public ServiceDirectory.Core.Entities.Organisation Build()
    {
        return _organisation;
    }
}
