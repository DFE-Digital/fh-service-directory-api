using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Organisation : EntityBase<string>, IAggregateRoot
{
    public Organisation() { }

    public Organisation(
        string id,
        OrganisationType organisationType,
        string name = default!,
        string? description = default!,
        string? logo = default!,
        string? uri = default!,
        string? url = default!,
        ICollection<Review>? reviews = default!,
        ICollection<Service>? services = default!
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
    public virtual ICollection<Review>? Reviews { get; set; } = new List<Review>();
    public virtual ICollection<Service>? Services { get; set; } = new List<Service>();

    public void Update(Organisation organisation)
    {
        Name = organisation.Name;
        Description = organisation.Description;
        Logo = organisation.Logo;
        Uri = organisation.Uri;
        Url = organisation.Url;
    }
}

