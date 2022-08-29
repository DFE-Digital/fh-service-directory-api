using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralFundings;

public class OpenReferralFunding : EntityBase<string>, IOpenReferralFunding, IAggregateRoot
{
    private OpenReferralFunding() { }
    public OpenReferralFunding(string id, string source)
    {
        Id = id;
        Source = source;
    }
    public string Source { get; init; } = default!;
}
