using fh_service_directory_api.core.Entities;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Builders;

public class ServicesBuilder
{
    private readonly OpenReferralService _openReferralService;

    public ServicesBuilder()
    {
        _openReferralService = new OpenReferralService();
    }

    public ServicesBuilder WithMainProperties(string id, string name, string? description, string? accreditations, DateTime? assured_date, string? attending_access, string? attending_type, string? deliverable_type, string? status, string? url, string? email, string? fees)
    {
        _openReferralService.Id = id;
        _openReferralService.Name = name;
        _openReferralService.Description = description;
        _openReferralService.Accreditations = accreditations;
        _openReferralService.Assured_date = assured_date;
        _openReferralService.Attending_access = attending_access;
        _openReferralService.Attending_type = attending_type;
        _openReferralService.Deliverable_type = deliverable_type;
        _openReferralService.Status = status;
        _openReferralService.Url = url;
        _openReferralService.Email = email;
        _openReferralService.Fees = fees;
        return this;
    }

    public ServicesBuilder WithServiceDelivery(ICollection<OpenReferralServiceDelivery>? serviceDelivery)
    {
        if (serviceDelivery != null && serviceDelivery.Any())
            _openReferralService.ServiceDelivery = serviceDelivery;
        return this;
    }

    public ServicesBuilder WithEligibility(ICollection<OpenReferralEligibility>? eligibilities)
    {
        if (eligibilities != null && eligibilities.Any())
            _openReferralService.Eligibilities = eligibilities;
        return this;
    }

    public ServicesBuilder WithContact(ICollection<OpenReferralContact>? contacts)
    {
        if (contacts != null && contacts.Any())
            _openReferralService.Contacts = contacts;
        return this;
    }

    public ServicesBuilder WithCostOption(ICollection<OpenReferralCost_Option> cost_options)
    {
        if (cost_options != null && cost_options.Any())
            _openReferralService.Cost_options = cost_options;
        return this;
    }

    public ServicesBuilder WithLanguages(ICollection<OpenReferralLanguage>? languages)
    {
        if (languages != null && languages.Any())
            _openReferralService.Languages = languages;
        return this;
    }

    public ServicesBuilder WithServiceAreas(ICollection<OpenReferralService_Area>? service_areas)
    {
        if (service_areas != null && service_areas.Any())
            _openReferralService.Service_areas = service_areas;
        return this;
    }

    public ServicesBuilder WithServiceAtLocations(ICollection<OpenReferralServiceAtLocation>? service_at_locations)
    {
        if (service_at_locations != null && service_at_locations.Any())
            _openReferralService.Service_at_locations = service_at_locations;
        return this;
    }

    public ServicesBuilder WithServiceTaxonomies(ICollection<OpenReferralService_Taxonomy>? service_taxonomys)
    {
        if (service_taxonomys != null && service_taxonomys.Any())
            _openReferralService.Service_taxonomys = service_taxonomys;
        return this;
    }

    public OpenReferralService Build()
    {
        return _openReferralService;
    }
}
