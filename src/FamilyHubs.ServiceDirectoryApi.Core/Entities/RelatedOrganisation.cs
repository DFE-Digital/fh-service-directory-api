using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace fh_service_directory_api.core.Entities;

public class RelatedOrganisation : EntityBase<string>, IAggregateRoot
{
    private RelatedOrganisation() { }
    public RelatedOrganisation(string id, string parentOrganisationId, string organisationId)
    {
        Id = id;
        ParentOrganisationId = parentOrganisationId;
        OrganisationId = organisationId;
    }

    public string ParentOrganisationId { get; set; } = default!;
    public string OrganisationId { get; set; } = default!;
}
