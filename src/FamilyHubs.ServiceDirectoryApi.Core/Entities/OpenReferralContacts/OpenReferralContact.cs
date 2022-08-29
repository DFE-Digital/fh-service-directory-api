using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralPhones;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralContacts;

public class OpenReferralContact : EntityBase<string>, IAggregateRoot, IOpenReferralContact
{
    private OpenReferralContact() { }
    public OpenReferralContact(string id, string title, string name, ICollection<OpenReferralPhone>? phones)
    {
        Id = id;
        Title = title;
        Name = name;
        Phones = phones;
    }
    public string Title { get; init; } = default!;
    public string Name { get; init; } = default!;
    public virtual ICollection<OpenReferralPhone>? Phones { get; init; }
}
