﻿using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralService_Taxonomy : EntityBase<string>, IAggregateRoot
{
    private OpenReferralService_Taxonomy() { }
    public OpenReferralService_Taxonomy(string id, string? linkId, OpenReferralTaxonomy? taxonomy)
    {
        Id = id;
        LinkId = linkId;
        Taxonomy = taxonomy;
    }
    public string? LinkId { get; set; }
    public OpenReferralTaxonomy? Taxonomy { get; set; }
    //public string OpenReferralParentId { get; set; } = default!;
    public string OpenReferralServiceId { get; set; } = default!;
}
