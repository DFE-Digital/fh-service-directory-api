﻿using FamilyHubs.ServiceDirectory.Shared.Entities;

namespace fh_service_directory_api.core.Interfaces.Events
{
    public interface IOpenReferralOrganisationCreatedEvent
    {
        IOpenReferralOrganisation Item { get; }
    }
}