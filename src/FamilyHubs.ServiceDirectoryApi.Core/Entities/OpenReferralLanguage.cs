using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralLanguage : EntityBase<string>, IAggregateRoot
{
    private OpenReferralLanguage() { }
    public OpenReferralLanguage(string id, string language)
    {
        Id = id;
        Language = language;
    }
    public string Language { get; set; } = default!;
    public string OpenReferralServiceId { get; set; } = default!;
}
