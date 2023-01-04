using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralOrganisation : EntityBase<string>, IOpenReferralOrganisation, IAggregateRoot
{
    public OpenReferralOrganisation() { }

    public OpenReferralOrganisation(
        string id,
        OrganisationType organisationType,
        string name = default!,
        string? description = default!,
        string? logo = default!,
        string? uri = default!,
        string? url = default!,
        ICollection<OpenReferralReview>? reviews = default!,
        ICollection<OpenReferralService>? services = default!
    )
    {
        Id = id;
        OrganisationType = organisationType;
        Name = name ?? default!;
        Description = description ?? string.Empty;
        Logo = logo ?? string.Empty;
        Uri = uri ?? string.Empty;
        Url = url ?? string.Empty;
        Reviews = reviews ?? default!;
        Services = services ?? default!;
    }

    public OrganisationType OrganisationType { get; set; } = default!;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? Logo { get; set; } = string.Empty;
    public string? Uri { get; set; } = string.Empty;
    public string? Url { get; set; } = string.Empty;
    public virtual ICollection<OpenReferralReview>? Reviews { get; set; } = new List<OpenReferralReview>();
    public virtual ICollection<OpenReferralService>? Services { get; set; } = new List<OpenReferralService>();
    public virtual ICollection<OpenReferralContactLink>? ContactLinks { get; set; } 

    public void Update(OpenReferralOrganisation openReferralOpenReferralOrganisation)
    {
        Name = openReferralOpenReferralOrganisation.Name;
        Description = openReferralOpenReferralOrganisation.Description;
        Logo = openReferralOpenReferralOrganisation.Logo;
        Uri = openReferralOpenReferralOrganisation.Uri;
        Url = openReferralOpenReferralOrganisation.Url;
    }
}

