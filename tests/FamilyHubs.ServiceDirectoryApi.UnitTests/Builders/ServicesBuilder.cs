using FamilyHubs.ServiceDirectory.Core.Entities;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Builders;

public class ServicesBuilder
{
    private readonly Service _service;

    public ServicesBuilder()
    {
        _service = new Service();
    }

    public ServicesBuilder WithMainProperties(
        string id, 
        string name, 
        string? description, 
        string? accreditations, 
        DateTime? assuredDate, 
        string? attendingAccess, 
        string? attendingType, 
        string? deliverableType, 
        string? status, 
        string? fees)
    {
        _service.Id = id;
        _service.Name = name;
        _service.Description = description;
        _service.Accreditations = accreditations;
        _service.AssuredDate = assuredDate;
        _service.AttendingAccess = attendingAccess;
        _service.AttendingType = attendingType;
        _service.DeliverableType = deliverableType;
        _service.Status = status;
        _service.Fees = fees;
        return this;
    }

    public ServicesBuilder WithServiceDelivery(ICollection<ServiceDelivery>? serviceDelivery)
    {
        if (serviceDelivery != null && serviceDelivery.Any())
            _service.ServiceDeliveries = serviceDelivery;
        return this;
    }

    public ServicesBuilder WithEligibility(ICollection<Eligibility>? eligibilities)
    {
        if (eligibilities != null && eligibilities.Any())
            _service.Eligibilities = eligibilities;
        return this;
    }

    public ServicesBuilder WithContact(ICollection<Contact>? contacts)
    {
        if (contacts != null && contacts.Any())
            _service.Contacts = contacts;
        return this;
    }

    public ServicesBuilder WithCostOption(ICollection<CostOption>? costOptions)
    {
        if (costOptions != null && costOptions.Any())
            _service.CostOptions = costOptions;
        return this;
    }

    public ServicesBuilder WithLanguages(ICollection<Language>? languages)
    {
        if (languages != null && languages.Any())
            _service.Languages = languages;
        return this;
    }

    public ServicesBuilder WithServiceAreas(ICollection<ServiceArea>? serviceAreas)
    {
        if (serviceAreas != null && serviceAreas.Any())
            _service.ServiceAreas = serviceAreas;
        return this;
    }

    public ServicesBuilder WithServiceAtLocations(ICollection<ServiceAtLocation>? serviceAtLocations)
    {
        if (serviceAtLocations != null && serviceAtLocations.Any())
            _service.ServiceAtLocations = serviceAtLocations;
        return this;
    }

    public ServicesBuilder WithServiceTaxonomies(ICollection<ServiceTaxonomy>? serviceTaxonomies)
    {
        if (serviceTaxonomies != null && serviceTaxonomies.Any())
            _service.ServiceTaxonomies = serviceTaxonomies;
        return this;
    }

    public Service Build()
    {
        return _service;
    }
}
