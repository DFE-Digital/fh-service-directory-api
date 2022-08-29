using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralPhones;

public class OpenReferralPhone : EntityBase<string>, IOpenReferralPhone, IAggregateRoot
{
    private OpenReferralPhone() { }
    public OpenReferralPhone(string id, string number)
    {
        Id = id;
        Number = number;
    }

    public string Number { get; init; } = default!;
}
