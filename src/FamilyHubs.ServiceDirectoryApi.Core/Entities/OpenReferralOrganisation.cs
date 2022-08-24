using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace FamilyHubs.ServiceDirectory.Shared.Entities;

public class OpenReferralOrganisation : EntityBase<string>, IOpenReferralOrganisation, IAggregateRoot
{
    private OpenReferralOrganisation() { }

    public OpenReferralOrganisation(
        string id,
        string name = default!,
        string? description = default!,
        string? logo = default!,
        string? uri = default!,
        string? url = default!,
        ICollection<OpenReferralReview>? reviews = default!,
        ICollection<OpenReferralService>? services = default!
    // TODO: Lock down the access to the collections
    //IEnumerable<IOpenReferralReview>? reviews = default,
    //IEnumerable<IOpenReferralService>? services = default
    )
    {
        Id = id;
        Name = name ?? default!;
        Description = description ?? string.Empty;
        Logo = logo ?? string.Empty;
        Uri = uri ?? string.Empty;
        Url = url ?? string.Empty;
        Reviews = (ICollection<IOpenReferralReview>)(reviews ?? default!);
        Services = (ICollection<IOpenReferralService>)(services ?? default!);
        //_reviews = (IList<IOpenReferralReview>)(reviews ?? new List<IOpenReferralReview>());
        //_services = (IList<IOpenReferralService>)(services ?? new List<IOpenReferralService>());
    }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; } = string.Empty;
    public string? Logo { get; private set; } = string.Empty;
    public string? Uri { get; private set; } = string.Empty;
    public string? Url { get; private set; } = string.Empty;
    public virtual ICollection<IOpenReferralReview>? Reviews { get; set; } = new List<IOpenReferralReview>();
    public virtual ICollection<IOpenReferralService>? Services { get; set; } = new List<IOpenReferralService>();
    // public IEnumerable<IOpenReferralReview> Reviews => _reviews;
    // public IEnumerable<IOpenReferralService> Services => _services;

    public void Update(IOpenReferralOrganisation openReferralOpenReferralOrganisation)
    {
        Name = openReferralOpenReferralOrganisation.Name;
        Description = openReferralOpenReferralOrganisation.Description;
        Logo = openReferralOpenReferralOrganisation.Logo;
        Uri = openReferralOpenReferralOrganisation.Uri;
        Url = openReferralOpenReferralOrganisation.Url;
    }

    //private readonly IList<IOpenReferralReview> _reviews = new List<IOpenReferralReview>();

    //private readonly IList<IOpenReferralService> _services = new List<IOpenReferralService>();

}

