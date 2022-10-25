using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace fh_service_directory_api.core.Entities;

public class RelatedOrganisation : EntityBase<string>, IRelatedOrganisation, IAggregateRoot
{
    private RelatedOrganisation() { }
    public RelatedOrganisation(string organisationId, string relatedOrganisationId)
    {
        OrganisationId = organisationId;
        RelatedOrganisationId = relatedOrganisationId;
    }

    public string OrganisationId { get; set; } = default!;
    public string RelatedOrganisationId { get; set; } = default!;
}
