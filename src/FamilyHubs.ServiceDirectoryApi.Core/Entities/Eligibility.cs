using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Eligibility : EntityBase<string>, IAggregateRoot
{
    private Eligibility() { }
    public Eligibility(
        string id, 
        string eligibilityDescription, 
        string? linkId, 
        int maximumAge, 
        int minimumAge, 
        ICollection<Taxonomy>? taxonomies)
    {
        Id = id;
        EligibilityDescription = eligibilityDescription;
        LinkId = linkId;
        MaximumAge = maximumAge;
        MinimumAge = minimumAge;
        Taxonomies = taxonomies;
    }
    public string EligibilityDescription { get; set; } = default!;
    public string? LinkId { get; set; }
    public int MaximumAge { get; set; }
    public int MinimumAge { get; set; }
    public ICollection<Taxonomy>? Taxonomies { get; set; }
    public string ServiceId { get; set; } = default!;
}
