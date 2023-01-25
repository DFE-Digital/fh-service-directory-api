using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace fh_service_directory_api.core.Entities;

public class Accessibility_For_Disabilities : EntityBase<string>, IAggregateRoot
{
    private Accessibility_For_Disabilities() { }
    public Accessibility_For_Disabilities(string id, string accessibility)
    {
        Id = id;
        Accessibility = accessibility;
    }
    public string Accessibility { get; init; } = default!;
    public string OpenReferralLocationId { get; set; } = default!;

}
