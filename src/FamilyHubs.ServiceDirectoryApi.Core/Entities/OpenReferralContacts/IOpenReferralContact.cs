using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralPhones;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralContacts;

public interface IOpenReferralContact
{
    string Name { get; init; }
    ICollection<OpenReferralPhone>? Phones { get; init; }
    string Title { get; init; }
}