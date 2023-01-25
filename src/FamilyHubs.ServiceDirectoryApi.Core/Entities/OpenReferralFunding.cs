using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralFunding : EntityBase<string>, IAggregateRoot
{
    private OpenReferralFunding() { }
    public OpenReferralFunding(string id, string source)
    {
        Id = id;
        Source = source;
    }
    public string Source { get; init; } = default!;

    //public string OpenReferralServiceId { get; set; } = default!;
}
