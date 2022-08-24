using FamilyHubs.ServiceDirectory.Shared.Interfaces.Entities;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Shared.Entities;

public class OpenReferralContact : EntityBase<string>, IOpenReferralContact, IAggregateRoot
{
    private OpenReferralContact() { }
    public OpenReferralContact(string id, string title, string name, ICollection<OpenReferralPhoneDto>? phones)
    {
        Id = id;
        Title = title;
        Name = name;
        Phones = phones;
    }
    public string Title { get; init; } = default!;
    public string Name { get; init; } = default!;
    public virtual ICollection<OpenReferralPhoneDto>? Phones { get; init; }

}
