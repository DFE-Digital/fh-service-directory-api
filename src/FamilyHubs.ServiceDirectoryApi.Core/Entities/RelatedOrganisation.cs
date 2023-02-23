using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class RelatedOrganisation : EntityBase<string>, IAggregateRoot
{
    private RelatedOrganisation() { }
    public RelatedOrganisation(string id,string organisationId, string relatedOrganisationId)
    {
        Id = id;
        OrganisationId = organisationId;
        RelatedOrganisationId = relatedOrganisationId;
    }

    public string OrganisationId { get; set; } = default!;
    public string RelatedOrganisationId { get; set; } = default!;
}
