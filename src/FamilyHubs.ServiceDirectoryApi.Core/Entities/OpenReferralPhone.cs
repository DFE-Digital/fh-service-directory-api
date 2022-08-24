using FamilyHubs.ServiceDirectory.Shared.Interfaces.Entities;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Shared.Entities;

public class OpenReferralPhoneDto : EntityBase<string>, IOpenReferralPhone, IAggregateRoot
{
    private OpenReferralPhoneDto() { }
    public OpenReferralPhoneDto(string id, string number)
    {
        Id = id;
        Number = number;
    }

    public string Number { get; init; } = default!;
}
