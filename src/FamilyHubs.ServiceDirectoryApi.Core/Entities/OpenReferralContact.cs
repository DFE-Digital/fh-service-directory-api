using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace FamilyHubs.ServiceDirectory.Shared.Entities;

public class OpenReferralContact : EntityBase<string>, IOpenReferralContact, IAggregateRoot
{
    private OpenReferralContact() { }
    public OpenReferralContact(string id, string title, string name, ICollection<IOpenReferralPhone>? phones)
    {
        Id = id;
        Title = title;
        Name = name;
        Phones = phones;
    }
    public string Title { get; init; } = default!;
    public string Name { get; init; } = default!;
    public virtual ICollection<IOpenReferralPhone>? Phones { get; init; }
}
