﻿using FamilyHubs.ServiceDirectory.Shared.Interfaces.Entities;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Shared.Entities;

public class OpenReferralLinktaxonomycollection : EntityBase<string>, IOpenReferralLinktaxonomycollection, IAggregateRoot
{
    private OpenReferralLinktaxonomycollection() { }
    public OpenReferralLinktaxonomycollection(string id, string link_id, string link_type)
    {
        Id = id;
        Link_id = link_id;
        Link_type = link_type;
    }
    public string Link_id { get; init; } = default!;
    public string Link_type { get; init; } = default!;
}
