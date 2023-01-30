using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace fh_service_directory_api.core.Entities;

//Remove this attribute when this entity starts being used
[ExcludeFromCodeCoverage]
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
