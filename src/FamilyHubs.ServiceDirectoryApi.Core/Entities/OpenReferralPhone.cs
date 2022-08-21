using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Interfaces.Entities;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralPhone : EntityBase<string>, IOpenReferralPhone
{
    private OpenReferralPhone() { }
    public OpenReferralPhone(string id, string number)
    {
        Id = id;
        Number = number;
    }

    public string Number { get; init; } = default!;
}
