﻿using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Interfaces.Entities;

public interface IOpenReferralOrganisation : IEntityBase<string>
{
    string? Description { get; }
    string? Logo { get; }
    string Name { get; }
    ICollection<OpenReferralReview>? Reviews { get; set; }
    ICollection<OpenReferralService>? Services { get; set; }
    ICollection<OpenReferralContactLink>? ContactLinks { get; set; }
    string? Uri { get; }
    string? Url { get; }
    OrganisationType OrganisationType { get; set; }
    void Update(OpenReferralOrganisation openReferralOpenReferralOrganisation);
}